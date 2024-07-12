namespace crud_redo.Models;
// dont forget to write the name space when making a new file : )

public class BooksEntity {

    // how do I set ID to be generative text like the ObjectID in mongo?
    //  Do so in the iteration on monday.
    public int Id { get; set;}
    public string Title { get; set;}
    public string Author { get; set;}
    public string Genre { get; set;}
    public string ISBN { get; set;}
    public DateOnly DatePublished { get; set; } 
}
