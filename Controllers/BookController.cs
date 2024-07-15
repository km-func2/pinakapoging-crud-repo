namespace crud_redo.Controllers;
using System.Threading.Tasks;

using System.Diagnostics   ;
using Microsoft.AspNetCore.Mvc;
using crud_redo.Models;
using crud_redo.Data;

public class BookController : Controller
{
    private ApplicationDbContext _db;

    public  BookController(ApplicationDbContext db){
        _db = db;
    }

    public IActionResult Index(){

        var books = _db.NewBooks.ToList();
        return View(books);
    }

    public IActionResult Create(){
        return View(); 
    }

    [HttpPost]
    public async Task<IActionResult> CreateBook(BookFormModel NewFormBook){
        string Base64Image;
        // books DB has changed into NewBooks as to suppor the base64 database
         using (var memoryStream = new MemoryStream())
        {
            await NewFormBook.ImageFile.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            Base64Image = Convert.ToBase64String(fileBytes);
        }

        // maps the book from the Forms to the DB NewBooksEntity format
        var formattedBook = new NewBooksEntity {
            Title = NewFormBook.Title,
            Author = NewFormBook.Author,
            Genre = NewFormBook.Genre,
            ISBN = NewFormBook.ISBN,
            DatePublished = NewFormBook.DatePublished,
            BookCoverBase64 = Base64Image
        };
        
        _db.NewBooks.Add(formattedBook);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        // return View();
        //  var book=_db.Books.Find(id);
        var book=_db.NewBooks.Find(id);
        
        if(book==null){
            return NotFound();
        }
         _db.NewBooks.Remove(book);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    // Helper Function to convert Base64 into IFormFile
    public static IFormFile Base64ToFormFile(string base64String, string fileName)
    {
        byte[] bytes = Convert.FromBase64String(base64String);
        MemoryStream stream = new MemoryStream(bytes);
        IFormFile file = new FormFile(stream, 0, bytes.Length, fileName, fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };
        return file;
    }
    public IActionResult Edit(int id)
    {

        var books=_db.NewBooks.Find(id);
        if(books==null){
            return NotFound();
        } 

        var formattedBook = new BookFormModel
        {
            Id = books.Id
            Title = books.Title,
            Author = books.Author,
            Genre = books.Genre,
            ISBN = books.ISBN,
            DatePublished = books.DatePublished,
            ImageFile = Base64ToFormFile(books.BookCoverBase64, "bookcover.png")
        };
        return View(formattedBook);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditBook(BookFormModel NewFormBook){
        
        string Base64Image;
        // books DB has changed into NewBooks as to suppor the base64 database
         using (var memoryStream = new MemoryStream())
        {
            await NewFormBook.ImageFile.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            Base64Image = Convert.ToBase64String(fileBytes);
        }

        // maps the book from the Forms to the DB NewBooksEntity format
        var formattedBook = new NewBooksEntity {
            Id = NewFormBook.Id,
            Title = NewFormBook.Title,
            Author = NewFormBook.Author,
            Genre = NewFormBook.Genre,
            ISBN = NewFormBook.ISBN,
            DatePublished = NewFormBook.DatePublished,
            BookCoverBase64 = Base64Image
        };
        
        _db.NewBooks.Update(formattedBook);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
 
}