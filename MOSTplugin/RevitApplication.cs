using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace MOSTplugin
{
    public class hello_tab : IExternalApplication
    {
        string assembly_path = Assembly.GetExecutingAssembly().Location;
        string tab_name = "MOST";
        string panel_name = "Листы и виды";

        public Result OnShutdown(UIControlledApplication application)
        {
            
            return Result.Succeeded;
           
        }

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(tab_name);
            RibbonPanel ribbonpanel = application.CreateRibbonPanel(tab_name, panel_name);
            PushButtonData button_rename_views = new PushButtonData(nameof(Commands), "переименовать \n виды", assembly_path, typeof(Commands).FullName);
            Image img_button_rename_views = Properties.Resources.btn1_icon_32x32;
            ImageSource imgSRC_button_rename_views = Convert(img_button_rename_views);
            button_rename_views.LargeImage = imgSRC_button_rename_views;
            ribbonpanel.AddItem(button_rename_views);

            RibbonPanel ribbonpanel2 = application.CreateRibbonPanel(tab_name, "TEST");
            PushButtonData button_renumber_lists = new PushButtonData(nameof(Commands), "перенумеровать \n листы", assembly_path, typeof(Auto_numbering).FullName);
            Image img_button_renumber_lists = Properties.Resources.btn1_icon_32x32;
            ImageSource imgSRC_button_renumber_lists = Convert(img_button_renumber_lists);
            button_renumber_lists.LargeImage = imgSRC_button_renumber_lists;
            ribbonpanel2.AddItem(button_renumber_lists);
            

            RibbonPanel ribbonpanel3 = application.CreateRibbonPanel(tab_name, "Перемычки");
            PushButtonData button_peremichki = new PushButtonData(nameof(Commands), "перемычки", assembly_path, typeof(Peremichki_command).FullName);
            Image img_button_peremichki = Properties.Resources.btn1_icon_32x32;
            ImageSource imgSRC_button_peremichki = Convert(img_button_peremichki);
            button_peremichki.LargeImage = imgSRC_button_peremichki;
            ribbonpanel3.AddItem(button_peremichki);
            ribbonpanel.AddSeparator();


            RibbonPanel SkittingWall_panel = application.CreateRibbonPanel(tab_name, "Отделка");
            PushButtonData SkittingWall_btn = new PushButtonData(nameof(Commands), "Отделка", assembly_path, typeof(SkittingWall_command).FullName);
            Image SkittingWaal_img = Properties.Resources.btn1_icon_32x32;
            ImageSource SkittingWall_imgSRC = Convert(SkittingWaal_img);
            SkittingWall_btn.LargeImage = SkittingWall_imgSRC; 
            SkittingWall_panel.AddItem(SkittingWall_btn);






            /*Uri button1_icon = new Uri(Path.Combine(Path.GetDirectoryName(assembly_path),"icons", "button1_icon.ico"));
            BitmapImage btn1_icon = new BitmapImage(button1_icon);
            button1.LargeImage = btn1_icon;
            ribbonpanel.AddItem(button1);*/


            return Result.Succeeded;
        }

        public BitmapImage Convert(Image img) {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;


            
            
            }
        
        }
    }
}
