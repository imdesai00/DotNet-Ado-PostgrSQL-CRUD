using adocursorpostgre;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection.PortableExecutable;

class Program
{
    static void Main()
    {
        var npgdb = new Npgdb();
        //using insert,delete,update in store procedure
        npgdb.insertdatasp(12, "Billie", "Eilish", 22, "Female", 88.99);
        npgdb.deletedatasp(7);
        npgdb.updatedatasp(11, "Lana", "DelRey");

        // perform get data using dataadapater(dataset,datatable) & datacommand(reader) by retutn table method
        npgdb.selectdatabytablemethod();

        Console.ReadKey();
    }
}