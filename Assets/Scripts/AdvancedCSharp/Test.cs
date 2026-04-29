using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.UI;
using UnityEngine.UI;
using System.Linq;
using System;


// misc note: 
// setting something equal to a field of a class constantly updates because 
// they all point to the same thing but structs, upon assignment, make a copy 
// so the fields are frozen as they are at that moment 

// if you use a struct for something like keeping up with the player's position, 
// then it won't properly update; however, a class would update. 

// structs are non-nullable types. 

// playerPos = new Transform(); <- class allocation, takes up a lot more resources than for a struct 
// playerPos = new Vector3(42, 1, 1); <- struck allocation takes less because it is on the stack whereas classes go on the heap.
// allocating classes a ton can be a performance hit. You can call new on a struct every frame and your performance won't suffer. 

// User Defines Types: class, record, enum, struct, tuple

// boilerplate needed to get the expanded decorators to work properly 
namespace System.Runtime.CompilerServices
{
    public interface IsExternalInit { }
}

class AdvancedCSharp
{
    // properties 
    private string name;
    // properties with implementation details
    public string Name
    {
        get => name; // lambda function
        private set
        {
            // value is a contextual keyword 
            name = value;
        }
    }


    // auto implemented properties 
    public int I { get; set; }
}


// creates a read only class, automatically gives you 
// a to string method and overrides the == operator
record MyPlayer(string name, int score);

// extension methods 
// ** you can add methods to existing classes ** 
// you should put these into their own file in reality 
// must be public, static, and a class
public static class MyExtensions
{
    // with a static class, all members must be static
    // this is its meaning in C# specificially, it is different in Java
    // 'this' placed on the first parameter is what actually makes it an extension method
    public static int CountChars(this string s, char target)
    {
        int totals = 0;
        foreach (char c in s)
        {
            if (c == target)
            {
                totals++;
            }
        }
        return totals;
    }

    // it knows what it is extending based on the type of the 'this' parameter
    public static void ChangeAlpha(this Image img, float alpha)
    {
        // have to make a copy 
        var color = img.color;
        // grab color 
        color.a = alpha;
        img.color = color; 
        
    }
}

public enum Colors
{
    RED, 
    GREEN, 
    BLUE, 
    ORANGE,
}

public class Test : MonoBehaviour
{
    private void Start()
    {
        // tuple creation - putting variable names to each member of this ad-hoc class
        (string name, int id) = ("Chandler", 34_342);
        
        // tuples make swapping easier, no need for a temp variable
        (int num1, int num2) = (12, 24);
        (num1, num2) = (num2, num1);

        // tuple instantiation as a data type (smart way to do it imo) 
        (int num1, int num2, float amount) x = (3, 4, 3.14f);
        // you can retrieve these values like this 
        print(x.num1);

        // generic implementation 
        Tuple<int, int, float> y = new(1, 2, 3.5f);
        // you cannot write to this implementation, which is better, tuples being immutable is the whole point. 
        // I also like this more because a generic implementation is more C++ ish :D 


        // s contains exactly 15 spaces. same as " " * 15 in Python
        var s = new string(' ', 15); 

        // gives collection of consecutive integers just like in Python 
        Enumerable.Range(0, 10);

        // gives an array of objects that is  all values in your enum
        // lets you iterate through all enum values 
        // can be used with finite state machines
        foreach (object c in System.Enum.GetValues(typeof(Colors)))
        {
            print((Colors)c); // casting it to its actual object type
        }

        Image img;
        //img.ChangeOpacity(0.5f);

        string msg = "Chandler's totally original message";
        msg.CountChars('u');

        //AdvancedCSharp ac = new() { Name = "another name", I = 45 };
        //ac.Name = "x";
        //print(ac.Name);
        // can use underscore in place of commas for larger numbers
        MyPlayer p = new("Bob", 65_984);
        MyPlayer p2 = new("Bob", 65_984);
        // keyword with lets you take everything and change just a few things 
        MyPlayer p3 = p with { name = "Robert" };

        // you do string interpolation 
        print($"P3's name is {p3.name}"); // you can call executable code in there like a toString
        print(p); // toString
        if (p == p2)
        {
            // == operator is overloaded 
            print("Players are equal");
        }

        // switch statements 
        // statement because you can do things but not return things
        switch (p2.score)
        {
            case 0:
                print("you suck");
                break;

            case 1:
                print("you got one point");
                break;
        }

        // switch expression 
        // you can return things, you could also just put print at the front 
        // like print(p2.score switch)
        var x2 = p2.score switch
        {
            0 => "you suck",
            1 => "you got one point",
            _ => "high score!"
        };
    }

    private void Update()
    {

    }
}
