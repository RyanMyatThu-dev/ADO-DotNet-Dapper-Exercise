-- Seed data for Books table
-- ~30 sample books covering Fiction, Non-Fiction, Science, History, Fantasy, Mystery, Romance, Sci-Fi
-- Run after schema.sql: psql -U postgres -d Books < scripts/seed.sql

INSERT INTO books (title, author, genre, description, publisheddate)
VALUES
    ('To Kill a Mockingbird', 'Harper Lee', 'Fiction', 'A novel about racial injustice in the Deep South', '1960-07-11'),
    ('1984', 'George Orwell', 'Fiction', 'A dystopian novel about totalitarianism', '1949-06-08'),
    ('Pride and Prejudice', 'Jane Austen', 'Romance', 'A romantic novel about manners and marriage', '1813-01-28'),
    ('The Great Gatsby', 'F. Scott Fitzgerald', 'Fiction', 'A story of wealth and love in the Jazz Age', '1925-04-10'),
    ('One Hundred Years of Solitude', 'Gabriel García Márquez', 'Fiction', 'A landmark of magical realism', '1967-06-05'),
    ('Brave New World', 'Aldous Huxley', 'Sci-Fi', 'A futuristic dystopian novel', '1932-01-01'),
    ('The Catcher in the Rye', 'J.D. Salinger', 'Fiction', 'A story of teenage rebellion and angst', '1951-07-16'),
    ('The Hobbit', 'J.R.R. Tolkien', 'Fantasy', 'A fantasy adventure about Bilbo Baggins', '1937-09-21'),
    ('Dune', 'Frank Herbert', 'Sci-Fi', 'An epic science fiction saga set on the desert planet Arrakis', '1965-08-01'),
    ('The Lord of the Rings', 'J.R.R. Tolkien', 'Fantasy', 'An epic high-fantasy trilogy', '1954-07-29'),
    ('Harry Potter and the Sorcerer''s Stone', 'J.K. Rowling', 'Fantasy', 'A young wizard discovers his magical heritage', '1997-06-26'),
    ('The Da Vinci Code', 'Dan Brown', 'Mystery', 'A thriller about secret societies and religious mysteries', '2003-03-18'),
    ('The Alchemist', 'Paulo Coelho', 'Fiction', 'A philosophical novel about following your dreams', '1988-01-01'),
    ('A Brief History of Time', 'Stephen Hawking', 'Science', 'A exploration of cosmology and the universe', '1988-03-01'),
    ('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', 'History', 'A survey of the history of the human species', '2011-01-01'),
    ('The Art of War', 'Sun Tzu', 'Non-Fiction', 'An ancient Chinese military treatise', '0005-01-01'),
    ('Thinking, Fast and Slow', 'Daniel Kahneman', 'Science', 'A book about the two systems of thought', '2011-10-25'),
    ('The Diary of a Young Girl', 'Anne Frank', 'History', 'The wartime diary of a Jewish girl in hiding', '1947-06-25'),
    ('The Silent Patient', 'Alex Michaelides', 'Mystery', 'A psychological thriller about a woman who stops speaking', '2019-02-05'),
    ('Where the Crawdads Sing', 'Delia Owens', 'Fiction', 'A coming-of-age story set in the marshlands', '2018-08-14'),
    ('Educated', 'Tara Westover', 'Non-Fiction', 'A memoir of a woman who grows up in a survivalist family', '2018-02-20'),
    ('The Martian', 'Andy Weir', 'Sci-Fi', 'An astronaut stranded on Mars fights to survive', '2011-01-01'),
    ('Gone Girl', 'Gillian Flynn', 'Mystery', 'A thriller about a wife''s disappearance', '2012-06-05'),
    ('The Notebook', 'Nicholas Sparks', 'Romance', 'A love story spanning decades', '1996-10-01'),
    ('A Game of Thrones', 'George R.R. Martin', 'Fantasy', 'The first book in the epic fantasy series A Song of Ice and Fire', '1996-08-01'),
    ('Cosmos', 'Carl Sagan', 'Science', 'A exploration of the universe and our place in it', '1980-01-01'),
    ('Meditations', 'Marcus Aurelius', 'Non-Fiction', 'A series of personal writings by the Roman Emperor', '0180-01-01'),
    ('The Shining', 'Stephen King', 'Fiction', 'A horror novel about a haunted hotel', '1977-01-28'),
    ('Neuromancer', 'William Gibson', 'Sci-Fi', 'A foundational cyberpunk novel', '1984-07-01'),
    ('The Picture of Dorian Gray', 'Oscar Wilde', 'Fiction', 'A philosophical novel about beauty and morality', '1890-07-01');
