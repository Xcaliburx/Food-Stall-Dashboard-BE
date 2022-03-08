using Binus.SampleWebAPI.Data.Repositories.Training.BookDB.MSSQL.App;
using Binus.SampleWebAPI.Model.Training.BookDB.MSSQL.App;
using Binus.WebAPI.Model.MSSQL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Binus.SampleWebAPI.Services.Training.BookDB.MSSQL.App
{
    public interface IBookService
    {
        Task<List<BookModel>> GetAllBook();
        BookModel GetOneBook(int BookID);
        Task<ExecuteResult> InsertBook(string BookName, string BookDesc, int BookQty);
        Task<ExecuteResult> InsertBookWithModel(BookModel Model);
        ExecuteResult UpdateBook(BookModel Model);
        ExecuteResult DeleteBook(BookModel Model);
        Task<ExecuteResult> BorrowBook(BookModel book, int UserID);
        List<BookModel> GetBorrowedBook(int UserID);
        //Task<ExecuteResult> InsertBorrowBook(BookModel book);
        //List<BookModel> GetAllBorrowedBook();

    }

    public class BookService : IBookService
    {
        private readonly IBookRepository _BookRepository;

        public BookService(IBookRepository _BookRepository)
        {
            this._BookRepository = _BookRepository;
        }


        public async Task<List<BookModel>> GetAllBook()
        {
            List<BookModel> ListBook = (await _BookRepository.ExecSPToListAsync("bn_BookDB_GetAllbook")).ToList();

            return ListBook;
        }

        public BookModel GetOneBook(int BookID)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@BookID", BookID)
            };

            BookModel Model = _BookRepository.ExecSPToSingle("bn_BookDB_GetOneBook @BookID", Param);

            return Model;
        }

        public async Task<ExecuteResult> InsertBook(string BookName, string BookDesc, int BookQty)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@BookName", BookName),
                new SqlParameter("@BookDesc", BookDesc),
                new SqlParameter("@BookQty", BookQty)
            };

            List<StoredProcedure> Data = new List<StoredProcedure>();
            Data.Add(new StoredProcedure
            {
                SPName = "bn_BookDB_InsertBook @BookName, @BookDesc, @BookQty",
                SQLParam = Param
            });

            ExecuteResult Result = (await _BookRepository.ExecMultipleSPWithTransactionAsync(Data));

            return Result;
        }

        public async Task<ExecuteResult> InsertBookWithModel(BookModel Model)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@BookName", Model.BookName),
                new SqlParameter("@BookDesc", Model.BookDesc),
                new SqlParameter("@BookQty", Model.BookQty)
            };

            List<StoredProcedure> Data = new List<StoredProcedure>();
            Data.Add(new StoredProcedure
            {
                SPName = "bn_BookDB_InsertBook @BookName, @BookDesc, @BookQty",
                SQLParam = Param
            });

            ExecuteResult Result = (await _BookRepository.ExecMultipleSPWithTransactionAsync(Data));

            return Result;
        }

        public ExecuteResult UpdateBook(BookModel Model)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@BookID", Model.BookID),
                new SqlParameter("@BookName", Model.BookName),
                new SqlParameter("@BookDesc", Model.BookDesc),
                new SqlParameter("@BookQty", Model.BookQty)
            };

            List<StoredProcedure> Data = new List<StoredProcedure>();
            Data.Add(new StoredProcedure
            {
                SPName = "bn_BookDB_UpdateBook @BookID, @BookName, @BookDesc, @BookQty",
                SQLParam = Param
            });

            ExecuteResult Result = _BookRepository.ExecMultipleSPWithTransaction(Data);

            return Result;
        }

        public ExecuteResult DeleteBook(BookModel Model)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@BookID", Model.BookID)
            };

            List<StoredProcedure> Data = new List<StoredProcedure>();
            Data.Add(new StoredProcedure
            {
                SPName = "bn_BookDB_DeleteBook @BookID",
                SQLParam = Param
            });

            ExecuteResult Result = _BookRepository.ExecMultipleSPWithTransaction(Data);

            return Result;
        }

        public async Task<ExecuteResult> BorrowBook(BookModel Model, int UserID)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@BookID", Model.BookID),
                new SqlParameter("@UserID", UserID)
            };

            List<StoredProcedure> Data = new List<StoredProcedure>();
            Data.Add(new StoredProcedure
            {
                SPName = "bn_BookDB_BorrowBook @UserID, @BookId",
                SQLParam = Param
            });

            ExecuteResult Result = (await _BookRepository.ExecMultipleSPWithTransactionAsync(Data));

            return Result;
        }

        //public async Task<ExecuteResult> InsertBorrowBook(BookModel book)
        //{
        //    var Param = new SqlParameter[]
        //    {
        //        new SqlParameter("@BookId", book.BookID)
        //    };

        //    List<StoredProcedure> Data = new List<StoredProcedure>();
        //    Data.Add(new StoredProcedure
        //    {
        //        SPName = "bn_BookDB_InsertBorrowBook @BookId",
        //        SQLParam = Param
        //    });

        //    ExecuteResult Result = (await _BookRepository.ExecMultipleSPWithTransactionAsync(Data));

        //    return Result;
        //}


        public List<BookModel> GetBorrowedBook(int UserID)
        {
            var Param = new SqlParameter[]
            {
                new SqlParameter("@UserID", UserID)
            };

            List<BookModel> ListBorrow = _BookRepository.ExecSPToList("bn_BookDB_GetBorrowedBook @UserID", Param).ToList();

            return ListBorrow;
        }

        //public List<BookModel> GetAllBorrowedBook()
        //{
        //    List<BookModel> ListBorrow = _BookRepository.ExecSPToList("bn_BookDB_GetAllBorrowedBook").ToList();

        //    return ListBorrow;
        //}


    }


}
