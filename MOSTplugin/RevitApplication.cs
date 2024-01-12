using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VCRevitRibbonUtil;
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
            
            

            //PushButtonData button_rename_views = new PushButtonData(nameof(Commands), "переименовать \n виды", assembly_path, typeof(Commands).FullName);
            Image img_button_rename_views = Properties.Resources.btn1_icon_32x32;
            ImageSource imgSRC_button_rename_views = Convert(img_button_rename_views);
            //button_rename_views.LargeImage = imgSRC_button_rename_views;
            //ribbonpanel.AddItem(button_rename_views);

            //RibbonPanel ribbonpanel2 = application.CreateRibbonPanel(tab_name, "Листы");
            //PushButtonData button_renumber_lists = new PushButtonData(nameof(Commands), "перенумеровать \n листы", assembly_path, typeof(Auto_numbering).FullName);
            Image img_button_renumber_lists = Properties.Resources.img_ListBtn;
            ImageSource imgSRC_button_renumber_lists = Convert(img_button_renumber_lists);
            //button_renumber_lists.LargeImage = imgSRC_button_renumber_lists;
            //ribbonpanel2.AddItem(button_renumber_lists);

            

            //RibbonPanel SkittingWall_panel = application.CreateRibbonPanel(tab_name, "Архитектура");
            //PushButtonData SkittingWall_btn = new PushButtonData(nameof(Commands), "Отделка", assembly_path, typeof(SkittingWall_command).FullName);
            Image SkittingWaal_img = Properties.Resources.img_SkittingWallBtn;
            ImageSource SkittingWall_imgSRC = Convert(SkittingWaal_img);
            //SkittingWall_btn.LargeImage = SkittingWall_imgSRC; 
            //SkittingWall_panel.AddItem(SkittingWall_btn);

            //RibbonPanel check_panel = application.CreateRibbonPanel(tab_name, "BIM");
            //PushButtonData check_btn = new PushButtonData(nameof(Commands), "Проверка", assembly_path, typeof(check).FullName);
            Image check_img = Properties.Resources.img_BIMcheck;
            ImageSource check_imgSRC = Convert(check_img);
            //check_btn.LargeImage = check_imgSRC;
            //check_panel.AddItem(check_btn);


            //RibbonPanel LintelBeam_panel = application.CreateRibbonPanel(tab_name, "Перемчки");
            //PushButtonData LintelBeam_btn = new PushButtonData(nameof(Commands), "Менеджер", assembly_path, typeof(Peremichki_command).FullName);
            Image LintelBeam_img = Properties.Resources.img2_LintelBeam;
            ImageSource LintelBeam_imgSRC = Convert(LintelBeam_img);
            //LintelBeam_btn.LargeImage = LintelBeam_imgSRC;
            Image DoorManager_img = Properties.Resources.img_DoorManager;
            ImageSource DoorManager_imgSRC = Convert(DoorManager_img);
            
            
            Image PinTabs_img = Properties.Resources.img_PinTabs;
            ImageSource PinTabs_imgSRC = Convert(PinTabs_img);
            
            
            Image RoomTag_img = Properties.Resources.img_RoomTag;
            ImageSource RoomTag_imgSRC = Convert(RoomTag_img);


            //PushButton pushButton1 = SkittingWall_panel.AddItem(SkittingWall_btn) as PushButton;
            //PushButton pushButton2 = LintelBeam_panel.AddItem(LintelBeam_btn) as PushButton;
            Ribbon ribbon = new Ribbon(application);
            ribbon.Tab("MOSTPlugin")
                .Panel("Виды")
                .CreateButton("btn1", // имя
                             "Переименовать \nвиды", // текст кнопки
                             typeof(Commands), // привязанная команда
                             btn => btn
                               .SetLargeImage(imgSRC_button_rename_views) // назначаем изображение
                              )
                .CreateButton("btn2", // имя
                             "Перенумеровать \nлисты", // текст кнопки
                             typeof(Auto_numbering), // привязанная команда
                             btn => btn
                               .SetLargeImage(imgSRC_button_renumber_lists) // назначаем изображение
                              );
                
            ribbon.Tab("MOSTPlugin")
               .Panel("Архитектура")
               .CreateButton("btn3", // имя
                             "Отделка", // текст кнопки
                             typeof(SkittingWall_command), // привязанная команда
                             btn => btn
                               .SetLargeImage(SkittingWall_imgSRC) // назначаем изображение
                              )
               .CreateButton("btn4", // имя
                             "Менеджер \nперемычек", // текст кнопки
                             typeof(Peremichki_command), // привязанная команда
                             btn => btn
                               .SetLargeImage(LintelBeam_imgSRC) // назначаем изображение
                              )
               .CreateButton("btn_Doors",
                                "Менеджер \nдверей",
                                typeof(DoorManager_command),
                                btn => btn.SetLargeImage(DoorManager_imgSRC))
               .CreateButton("btn_RoomTags",
                                "Марки \nпомещений",
                                typeof(PlaceRoomTag_command),
                                btn => btn.SetLargeImage(RoomTag_imgSRC));


            ribbon.Tab("MOSTPlugin")
                .Panel("BIM")
                .CreateButton("btn5", // имя
                             "Сравнение \nверсий", // текст кнопки
                             typeof(check), // привязанная команда
                             btn => btn
                               .SetLargeImage(check_imgSRC) // назначаем изображение
                              )
                .CreateButton("btn_PinTabs", // имя
                             "Закрепить \nэлементы", // текст кнопки
                             typeof(PinTabs_command), // привязанная команда
                             btn => btn
                               .SetLargeImage(PinTabs_imgSRC) // назначаем изображение
                );





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
