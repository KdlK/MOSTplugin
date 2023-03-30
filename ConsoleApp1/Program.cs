// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;

/*Console.WriteLine("Hello, World!");
List<string> list = new List<string>() {"v","t","k" };
foreach (string el in list)
    Console.WriteLine(el);
change(list);
foreach(string el in list)
    Console.WriteLine(el);
Console.ReadLine();*/
int a = 1;
Console.WriteLine(a);
test(ref a);
Console.WriteLine(a);
Console.ReadLine();


void test(ref int a) {
    a = 10;
    

}

void change(List<string> list) { 
    List<string> list2 = new List<string>(list);
    list2.Add("v");


}