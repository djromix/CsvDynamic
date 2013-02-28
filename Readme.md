###CsvDynamic

This is a small library I built to take a CSV file and convert it for use in C# using dynamic objects. This can
be used for using LINQ with CSV files in a snap. It's pretty simple to use:

```c#
var result = CsvDynamic.ReadCsv(@"SomeFile.csv");
var amounts = result.Select(r => r.Amount);
```

The first row of the CSV file is assumed to be the header. The properties of the objects in the list that is 
returned are derived from the contents of the header row, stripped of all non-alphanumeric characters (ex. "Total Deliveries"
is now TotalDeliveries).

It's also easy to use for mapping to your classes. You can use a function that generates a class, or you can
add a constructor that takes in a dynamic to your class:

```c#
public class SampleItem
{
    public SampleItem(dynamic item)
    {
        Date = DateTime.Parse(item.Date.Trim('\"'));
        Description = item.Description.Trim('\"');
        OriginalDescription = item.OriginalDescription.Trim('\"');
        Amount = decimal.Parse(item.Amount.Trim('\"'));
        TransactionType = item.TransactionType.Trim('\"');
        Category = item.Category.Trim('\"');
        AccountName = item.AccountName.Trim('\"');
        Labels = item.Labels.Trim('\"');
        Notes = item.Notes.Trim('\"');
    }

    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string OriginalDescription { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; }
    public string Category { get; set; }
    public string AccountName { get; set; }
    public string Labels { get; set; }
    public string Notes { get; set; }
}
```

Using the above class, generating a list of SampleItems is easy:

```c#
var result = CsvDynamic.ReadCsv(@"SomeFile.csv", i => new SampleItem(i));
var amounts = result.Select(r => r.Amount);
```

Not only does this give you back Intellisense, but it lets you scrub the CSV data of unneeded characters.

#### Extension Methods

If you already have the CSV loaded into an array of strings, you can simply call this method on it to get the same result:

	var items = fileContents.ConvertFromCsv();

Like ReadCsv, this can also take in a mapping function in the same way.