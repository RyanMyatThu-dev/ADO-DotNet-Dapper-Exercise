-- Database Schema — Book Management
-- PostgreSQL 17, standalone (no EF Core)
-- Run: psql -U postgres -d Books < scripts/schema.sql

DROP TABLE IF EXISTS "Books" CASCADE;

CREATE TABLE Books (
    Id            SERIAL        PRIMARY KEY,
    Title         VARCHAR(200)  NOT NULL,
    Author        VARCHAR(100)  NOT NULL,
    Genre         VARCHAR(50)   NULL,
    Description   VARCHAR(1000) NULL,
    PublishedDate TIMESTAMP     NULL,
    IsDeleted     BOOLEAN       NOT NULL DEFAULT FALSE,
    CreatedAt     TIMESTAMP     NOT NULL DEFAULT NOW(),
    UpdatedAt     TIMESTAMP     NOT NULL DEFAULT NOW()
);

CREATE INDEX IX_Books_Title  ON Books (Title);
CREATE INDEX IX_Books_Author ON Books (Author);
