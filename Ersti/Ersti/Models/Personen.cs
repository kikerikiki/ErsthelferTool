using System;
using System.Collections.Generic;

namespace Ersti.Models;

public partial class Personen
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Gebäude { get; set; } = null!;

    public int Stock { get; set; }

    public string Raum { get; set; } = null!;

    public string Aufgabe { get; set; } = null!;

    public int Klappe { get; set; }
}
