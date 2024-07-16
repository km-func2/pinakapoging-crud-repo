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
    // public static IFormFile Base64ToFormFile(string base64String, string fileName)
    // {
    //     byte[] bytes = Convert.FromBase64String(base64String);
    //     MemoryStream stream = new MemoryStream(bytes);
    //     IFormFile file = new FormFile(stream, 0, bytes.Length, fileName, fileName)
    //     {
    //         Headers = new HeaderDictionary(),
    //         ContentType = "application/octet-stream"
    //     };
    //     return file;
    // }
    public async Task<IActionResult> Edit(int id)
    {
        var books= await _db.NewBooks.FindAsync(id);
        if(books==null){
            return NotFound();
        } 
 
        return View(books);
    }
    
    [HttpPost]
    public async Task<IActionResult> EditBook(NewBooksEntity NewFormBook){
        var formattedBook = new NewBooksEntity{};
 
        // string Base64Image;
        // books DB has changed into NewBooks as to suppor thebase64 database
        // using (var memoryStream = new MemoryStream())
        // {
        //     await NewFormBook.ImageFile.CopyToAsync(memoryStream);
        //     var fileBytes = memoryStream.ToArray();
        //     Base64Image = Convert.ToBase64String(fileBytes);
        // }



        // maps the book from the Forms to the DBNewBooksEntity format
        formattedBook = new NewBooksEntity {
            Id = NewFormBook.Id,
            Title = NewFormBook.Title,
            Author = NewFormBook.Author,
            Genre = NewFormBook.Genre,
            ISBN = NewFormBook.ISBN,
            DatePublished = NewFormBook.DatePublished,
            BookCoverBase64 = NewFormBook.BookCoverBase64
        };  
        _db.NewBooks.Update(formattedBook);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ViewBook(int id){
        
        var book= await _db.NewBooks.FindAsync(id);
        if (book==null){
            return NotFound();
        }
        return View(book);   
    }

    public async Task<IActionResult> ViewBookImg(int id){
        
        var book= await _db.NewBooks.FindAsync(id);
        if (book==null){
            return NotFound();
        }
        return View(book);   
    }
}