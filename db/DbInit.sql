CREATE DATABASE "D:\\fbdata\\BOOKSDB.fdb"
USER 'SYSDBA' PASSWORD 'masterkey'
DEFAULT CHARACTER SET UTF8;

-- Create table Publisher
CREATE TABLE Publisher (
    id INTEGER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) CHARACTER SET UTF8 NOT NULL
);

-- Create table Author
CREATE TABLE Author (
    id INTEGER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    name VARCHAR(255) CHARACTER SET UTF8 NOT NULL,
    surname VARCHAR(255) CHARACTER SET UTF8 NOT NULL
);

-- Create table Book
CREATE TABLE Book (
    id INTEGER GENERATED BY DEFAULT AS IDENTITY PRIMARY KEY,
    title VARCHAR(255) CHARACTER SET UTF8 NOT NULL,
    acquirement_date DATE,
    total_pages INTEGER,
    on_page INTEGER,
    times_read INTEGER DEFAULT 0,
    notes BLOB SUB_TYPE TEXT CHARACTER SET UTF8,
    description BLOB SUB_TYPE TEXT CHARACTER SET UTF8,
    keywords VARCHAR(500) CHARACTER SET UTF8,
    rating INTEGER CHECK (rating BETWEEN 0 AND 10),
    author_id INTEGER NOT NULL,
    publisher_id INTEGER NOT NULL,
    FOREIGN KEY (author_id) REFERENCES Author (id) ON DELETE CASCADE,
    FOREIGN KEY (publisher_id) REFERENCES Publisher (id) ON DELETE CASCADE
);

-- Insert default publishers
INSERT INTO Publisher (name) VALUES ('Albatros Media');
INSERT INTO Publisher (name) VALUES ('Grada Publishing');
INSERT INTO Publisher (name) VALUES ('Host Brno');

-- Insert default authors
INSERT INTO Author (name, surname) VALUES ('Karel', 'Čapek');
INSERT INTO Author (name, surname) VALUES ('Božena', 'Němcová');
INSERT INTO Author (name, surname) VALUES ('Franz', 'Kafka');

-- Insert default books
INSERT INTO Book (title, acquirement_date, total_pages, on_page, times_read, notes, description, keywords, rating, author_id, publisher_id)
VALUES ('R.U.R.', '2025-01-01', 96, 0, 0, 'Moc se na tuto knížku těším.', 'Drama o robotech a lidech.', 'sci-fi, roboti, drama', 8, 1, 1);
INSERT INTO Book (title, acquirement_date, total_pages, on_page, times_read, notes, description, keywords, rating, author_id, publisher_id)
VALUES ('Babička', '2025-01-02', 243, 50, 1, 'Nádherná pasáž o přírodě a vztazích.', 'Klasický román české literatury.', 'rodina, příroda, venkov', 9, 2, 2);
INSERT INTO Book (title, acquirement_date, total_pages, on_page, times_read, notes, description, keywords, rating, author_id, publisher_id)
VALUES ('Proměna', '2025-01-03', 72, 15, 2, 'Intenzivní příběh o proměně a izolaci.', 'Příběh o proměně člověka v hmyz.', 'existencialismus, Kafka, povídka', 10, 3, 3);
