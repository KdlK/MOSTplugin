// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var l1 = new List<int>() { 1, 2, 3, 4, 5, 2, 2, 2, 4, 4, 4, 1 };
foreach (var l in l1)
{
    Console.WriteLine(l);
}
var g = l1.GroupBy(i => i);
Console.WriteLine("__________________________________");
foreach (var l in g)
{
    Console.WriteLine(l.Key);
}
Console.WriteLine("__________________________________");
foreach (var grp in g)
{
    Console.WriteLine("{0} {1}", grp.Key, grp.Count());
}