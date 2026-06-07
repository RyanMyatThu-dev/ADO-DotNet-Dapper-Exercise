-- Database Schema — Book Management
-- PostgreSQL 17, standalone (no EF Core)
-- Run: psql -U postgres -d Books < scripts/schema.sql

DROP TABLE IF EXISTS books CASCADE;

CREATE TABLE books (
    id            SERIAL        PRIMARY KEY,
    title         VARCHAR(200)  NOT NULL,
    author        VARCHAR(100)  NOT NULL,
    genre         VARCHAR(50)   NULL,
    description   VARCHAR(1000) NULL,
    publisheddate TIMESTAMP     NULL,
    isdeleted     BOOLEAN       NOT NULL DEFAULT FALSE,
    createdat     TIMESTAMP     NOT NULL DEFAULT NOW(),
    updatedat     TIMESTAMP     NOT NULL DEFAULT NOW()
);

CREATE INDEX ix_books_title  ON books (title);
CREATE INDEX ix_books_author ON books (author);
