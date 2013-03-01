###CsvDynamic

This is a small library I built to take a CSV file and convert it for use in C# using dynamic objects. This can
be used for using LINQ with CSV files in a snap. It's pretty simple to use:

```c#
var result = CsvDynamic.Convert(@"SomeFile.csv");
var amounts = result.Select(r => r.Amount);
```

The Convert function can handle string arrays, file paths, or Stream objects.

The first row of the CSV file is assumed to be the header. The properties of the objects in the list that is 
returned are derived from the contents of the header row, stripped of all non-alphanumeric characters (ex. "Total Deliveries"
is now TotalDeliveries).

Of course, you lose Intellisense when you use dynamic objects. However, it's also easy to use for mapping to your classes. 
You can use a function that generates a class, or you can add a constructor that takes in a dynamic object to your class:

```c#
public class SampleItem
{
    public SampleItem(dynamic item)
    {
        Date = DateTime.Parse(item.Date);
        Description = item.Description;
        OriginalDescription = item.OriginalDescription;
        Amount = decimal.Parse(item.Amount);
        TransactionType = item.TransactionType;
        Category = item.Category;
        AccountName = item.AccountName;
        Labels = item.Labels;
        Notes = item.Notes;
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
var result = CsvDynamic.Convert(@"SomeFile.csv", i => new SampleItem(i));
var amounts = result.Select(r => r.Amount);
```