﻿using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View = Autodesk.Revit.DB.View;

namespace MOSTplugin.LintelBeam
{
    internal class Logic
    {
        static IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(Data.Doc).OfClass(typeof(ViewFamilyType))
                                                      let type = elem as ViewFamilyType
                                                      where type.ViewFamily == ViewFamily.Section
                                                      select type;
        static private List<Element> GetLintelBeams() {
            List<Element> GenericModelCollector = new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_GenericModel).ToElements().ToList();

            List<Element> FilteredCollector = (from generic in GenericModelCollector
                                               where
                                               (Data.Doc.GetElement(generic.GetTypeId()) as ElementType)?
                                               .LookupParameter(Params.PMotherParameterName)?
                                               .AsString().Contains(Params.PMotherParameterValue) == true
                                               select generic).ToList();
            return FilteredCollector;


        }
        public static List<Model> CreateModelList() {
            Transaction t = new Transaction(Data.Doc, "MOSTPlugin: код перемычки");
            t.Start();
            List<Element> Elements = GetLintelBeams();
            List<IGrouping<string, Element>> GroupedElements = Elements.GroupBy(g => GetCode(g)).ToList();
            List<Model> ModelList = new List<Model>();
            foreach (IGrouping<string, Element> group in GroupedElements) {
                Model model = new Model();
                model.code = GetCode(group.First());
                foreach (Element el in group) {
                    model.LintelBeams.Add(el);
                }
                model.SetCode();
                ModelList.Add(model);
            }
            t.Commit();
            return ModelList;
        }

        static private string GetCode(Element _Element) {
            string mark = "";
            string SubName1Value = _Element.LookupParameter(Params.Sub1ParameterName)?.AsString();
            string SubName2Value = _Element.LookupParameter(Params.Sub2ParameterName)?.AsString();
            string SubName3Value = _Element.LookupParameter(Params.Sub3ParameterName)?.AsString();
            string SubName4Value = _Element.LookupParameter(Params.Sub4ParameterName)?.AsString();
            string SubName5Value = _Element.LookupParameter(Params.Sub5ParameterName)?.AsString();
            string SubName6Value = _Element.LookupParameter(Params.Sub6ParameterName)?.AsString();
            string WidthParameterValue = _Element.LookupParameter(Params.WidthParameterName)?.AsValueString();
            string LenghtParameterValue = _Element.LookupParameter(Params.LenghtParameterName)?.AsValueString();
            string IndentParameterValue = _Element.LookupParameter(Params.IndentParameterName)?.AsValueString();
            string OffsetParameterValue = _Element.LookupParameter(Params.OffsetParameterName)?.AsValueString();
            string PType = null;
            if ((Data.Doc.GetElement(_Element.GetTypeId()) as ElementType)?.LookupParameter(Params.PMotherParameterName).AsString().Contains("металлическая") == true)
                PType = "1";
            else if ((Data.Doc.GetElement(_Element.GetTypeId()) as ElementType)?.LookupParameter(Params.PMotherParameterName).AsString().Contains("железобетонная") == true)
                PType = "0";
            else PType = "???";
            List<string> SubNameValues = new List<string>() { SubName1Value, SubName2Value, SubName3Value, SubName4Value, SubName5Value, SubName6Value };


            int count = 1;
            if (_Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger() == 1)
            {
                mark = mark + String.Format("{0}_{1}__", count, SubNameValues[0]);
            }
            for (int i = 1; i < _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger(); i++)
            {
                if (SubNameValues[i] == SubNameValues[i - 1])
                {
                    count++;
                    if (i == _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger() - 1)
                    {
                        mark = mark + String.Format("{0}_{1}__", count, SubNameValues[i]);
                    }
                }
                else
                {
                    mark = mark + String.Format("{0}_{1}__", count, SubNameValues[i - 1]);
                    count = 1;
                    if (i == _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger() - 1)
                    {
                        mark = mark + String.Format("{0}_{1}__", count, SubNameValues[i]);
                    }
                }
            }
            string sub_mark = _Element.LookupParameter("М_Субкомпоненты_Кол-во").AsInteger().ToString() + "__" + mark;



            return string.Format("{0}__{1}x{2}__{3}{4}__{5}", PType, WidthParameterValue?.ToString(), LenghtParameterValue?.ToString(), sub_mark, IndentParameterValue, IndentParameterValue);
        }
        static public void CreateSectionView(Model model){
            Element el = model.LintelBeams.First();
            BoundingBoxXYZ sectionBox = SectionViewBB(el);
            List<Element> ViewList = (from view in new FilteredElementCollector(Data.Doc).OfCategory(BuiltInCategory.OST_Views) 
                                     let v = view as View 
                                     where v.ViewType.ToString() =="Section" 
                                     select view).ToList();
           

            //Transaction trans = new Transaction(Data.Doc, "Create section view");
            //trans.Start();


            ViewSection viewSection = ViewSection.CreateSection(Data.Doc, viewFamilyTypes.First().Id, sectionBox);
            bool t = true;
            foreach (Element elem in ViewList) {
                if (elem.Name == model.code) {
                    Data.Doc.Delete(elem.Id);
                    viewSection.Name = model.code;
                    model.Section = viewSection;
                    t = false;
                    break;
                }
            
            }
            //viewSection.LookupParameter("Уровень детализации") = 3;
            if (t) {
                viewSection.Name = model.code;
                model.Section = viewSection;
            }

            //trans.Commit();

        }
        static public BoundingBoxXYZ SectionViewBB(Element el) {
            LocationPoint lp = el.Location as LocationPoint;
            XYZ XYZ_location = lp.Point;
            double element_widthh = el.LookupParameter("ADSK_Размер_Ширина").AsDouble();
            XYZ VectorToAdd = new XYZ(element_widthh, 0, 0);
            XYZ XYZ_location_2 = XYZ_location.Add(VectorToAdd);

            Curve curve = Line.CreateBound(XYZ_location, XYZ_location_2) as Curve;
            Transform RotateFlipType = Transform.CreateRotationAtPoint(XYZ.BasisZ, lp.Rotation, XYZ_location);
            Curve rotatedCurve = curve.CreateTransformed(RotateFlipType);

            Transform curveTransform = rotatedCurve.ComputeDerivatives(0.5, true);
            XYZ origin = curveTransform.Origin;
            XYZ viewdir = curveTransform.BasisX.Normalize();
            XYZ up = XYZ.BasisZ;
            XYZ right = up.CrossProduct(viewdir);

            // Set up view transform, assuming wall's "up" 
            // is vertical. For a non-vertical situation 
            // such as section through a sloped floor, the 
            // surface normal would be needed

            Transform transform = Transform.Identity;
            transform.Origin = origin;
            transform.BasisX = right;
            transform.BasisY = up;
            transform.BasisZ = viewdir;
            //Transform RotateFlipType = Transform.CreateRotationAtPoint(XYZ.BasisY, 6, XYZ_location);
            BoundingBoxXYZ sectionBox = new BoundingBoxXYZ();
            sectionBox.Transform = transform;
            //sectionBox.Transform = RotateFlipType;
            sectionBox.Min = new XYZ(-3.5, 0, 0);
            sectionBox.Max = new XYZ(element_widthh / 304.8 + 5, 2, 1);
            return sectionBox;



        }
        static public void CreateViewImage(Model model) {
            Transaction t = new Transaction(Data.Doc, "go");
            Data.UIdoc.ActiveView = model.Section;
            t.Start();
            ImageExportOptions options = new ImageExportOptions()
            {
                ViewName = model.Section.Name

            };
            Data.Doc.SaveToProjectAsImage(options);
            
            
            t.Commit();
            


        }
        
    }
}
