using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace odevv
{



    class Program
    {
        static List<Book> Books = new List<Book>();
        static void Main(string[] args)
        {
            while (true)
            {
                

                Console.WriteLine("-----------");
                Console.WriteLine("1-) Kütüphaneye kitap ekle");
                Console.WriteLine("2-) Kitap listesini gör");
                Console.WriteLine("3-) Kitap ara");                                                            
                Console.WriteLine("4-) Kitap ödünç al");
                Console.WriteLine("5-) Kitap iade et");
                Console.WriteLine("6-) Süresi geçmiş kitapları görüntüle");
                Console.WriteLine("");
                Console.WriteLine("'0'ile çıkış yapabilirsiniz");
                Console.WriteLine("-----------");
                int secim = Convert.ToInt32(Console.ReadLine());

                switch (secim)
                {
                    case 1:
                        Addbook();
                        break;
                    case 2:
                        ShowBook();
                        break;
                    case 3:
                        SearchBook();
                        break;
                    case 4:
                        BorrowBook();
                        break;
                    case 5:
                        ReturnBook();
                        break;
                    case 6:
                        OverdueBooks();
                        break;
                    case 0:
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Yanlış seçim girdiniz.");
                        break;

                }

            }
        }

        static void Addbook()
        {
            Console.WriteLine("Kitap Adı giriniz.");
            string bookname = Console.ReadLine();
            Console.WriteLine("Yazar Adı giriniz.");
            string author = Console.ReadLine();
            Console.WriteLine("Kategori Giriniz.");
            string category = Console.ReadLine();
            Console.WriteLine("Adet sayısı giriniz");
            int count = Convert.ToInt32(Console.ReadLine());


            Book book = new Book(bookname, author, category, count);
            Books.Add(book);

            Console.WriteLine("Kitabınız girilmiştir.");
            Console.WriteLine("--------");
            Console.WriteLine($"{book.ISBN} - {book.Title} - {book.Author} - {book.Category} - {book.AvailableCopies} - {book.BorrowedCopies} ");


        }
        static void ShowBook()
        {

            Console.WriteLine("Kitaplar");
            Console.WriteLine("-----------");
            Console.WriteLine("ISBN - Kitap adı - Yazar adı - Kategori adı - Mevcut kopya sayısı - Ödünç alınan kitap sayısı");
            Console.WriteLine("-----------");
            foreach (Book book in Books)
            {
                Console.WriteLine($"ISBN:{book.ISBN} - {book.Title} - {book.Author} - {book.Category} - {book.AvailableCopies} - {book.BorrowedCopies} ");
            }
        }

        static void SearchBook()
        {

            Console.Write("Aranacak Kitap Adı, Yazar adı veya Kategoriyi yazınız: ");
            string search = Console.ReadLine();

            var results = Books.FindAll(book => book.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                || book.Author.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                book.Category.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);

            if (results.Count > 0)
            {

                Console.WriteLine("Kitaplar getiriliyor:");
                Console.WriteLine("----------------");
                Console.WriteLine("ISBN - Kitap adı - Yazar adı - Kategori adı - Mevcut kopya sayısı - Ödünç alınan kitap sayısı");
                foreach (Book book in results)
                {

                    Console.WriteLine($"ISBN:{book.ISBN} - {book.Title} - {book.Author} - {book.Category} - {book.AvailableCopies} - {book.BorrowedCopies}");
                    Console.WriteLine("------");
                }
            }
            else
            {
                Console.WriteLine("Aramada bir sonuç bulunamadı.");
            }

        }

        static void BorrowBook()
        {
            Console.Write("Ödünç alınacak kitabın ISBN giriniz. ");
            int isbn = Convert.ToInt32(Console.ReadLine());


            Book borrowedBook = Books.Find(book =>
       book.ISBN == isbn && book.AvailableCopies > 0
   );


            if (borrowedBook != null)
            {
                borrowedBook.BorrowBook();
                Console.WriteLine($"{borrowedBook.Title} kitabı ödünç alındı.");
                Console.WriteLine($"Ödünç alınan tarih {DateTime.Now}");
            }
            else
            {
                Console.WriteLine("Ödünç alınamadı!! Kitap bulunamadı veya mevcut kopya yok.");
            }
        }

        static void ReturnBook()
        {
            Console.Write("İade edilecek Kitap ISBN giriniz: ");
            int returnisbn = Convert.ToInt32(Console.ReadLine());

            Book returnedbook = Books.Find(book => book.ISBN == returnisbn);
            if (returnedbook != null)
            {
                returnedbook.ReturnBook();
                Console.WriteLine($"{returnedbook.Title} kitabı iade edildi.");
            }
            else
            {
                Console.WriteLine("İade edilemedi! hatalı ISBN girildi.");
            }

        }
        static void OverdueBooks()
        {
            Console.WriteLine("Süresi Geçmiş Kitaplar:");
            foreach (Book book in Books)
            {
                if (book.IsOverdue())
                {
                    Console.WriteLine($"ISBN:{book.ISBN} - {book.Title} - {book.Author} - {book.Category} - {book.AvailableCopies} - {book.BorrowedCopies}");
                }
            }
        }
    }
        class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }
            public string Category { get; set; }
            public int ISBN { get; set; }
            public int AvailableCopies { get; set; }
            public int BorrowedCopies { get; set; }
            
            public List<DateTime> BorrowedDates { get; set; }



            public Book(string title, string author, string category, int copycount)
            {
                Title = title;
                Author = author;
                Category = category;
                if(copycount>0)
                {
                    AvailableCopies = copycount;
                }
                else
                {
                    AvailableCopies = 1;
                }
                
                BorrowedCopies = 0;


                Random random = new Random();
                ISBN = random.Next(1, 250000);

                BorrowedDates = new List<DateTime>();
            }

            public void BorrowBook()
            {
                if (AvailableCopies > 0)
                {
                    AvailableCopies--;
                    BorrowedDates.Add(DateTime.Now);
                }
            }

            public void ReturnBook()
            {
                AvailableCopies++;
            }

            public bool IsOverdue()
            {

                int loanPeriodInDays = 7;
                foreach (DateTime borrowedDate in BorrowedDates)
                {
                    if (DateTime.Now - borrowedDate > TimeSpan.FromDays(loanPeriodInDays))
                    {
                        return true;
                    }
                }
                return false;
            }

        }

    }