-- Creator:       MySQL Workbench 6.0.7/ExportSQLite plugin 2009.12.02
-- Author:        Guru
-- Caption:       New Model
-- Project:       Name of the project
-- Changed:       2013-11-09 23:31
-- Created:       2013-11-02 01:04
PRAGMA foreign_keys = OFF;

-- Schema: musicplayer
BEGIN;
CREATE TABLE "playlists"(
  "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  "title" VARCHAR(45) NOT NULL
);
CREATE TABLE "directories"(
  "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  "path" VARCHAR(45) NOT NULL,
  "last_write_time" INTEGER NOT NULL,
  CONSTRAINT "path_UNIQUE"
    UNIQUE("path")
);
CREATE TABLE "songs"(
  "id" INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
  "path" VARCHAR(45) NOT NULL,
  "artist" VARCHAR(45),
  "album" VARCHAR(45),
  "title" VARCHAR(45),
  "year" INTEGER,
  "genre" VARCHAR(45),
  "track_no" INTEGER,
  "rating" INTEGER,
  "length" INTEGER,
  "id_directory" INTEGER NOT NULL,
  CONSTRAINT "path_UNIQUE"
    UNIQUE("path"),
  CONSTRAINT "id_directory"
    FOREIGN KEY("id_directory")
    REFERENCES "directories"("id")
    ON DELETE CASCADE
    ON UPDATE CASCADE
);
CREATE INDEX "songs.id_directory_idx" ON "songs"("id_directory");
CREATE TABLE "playlist_content"(
  "id_song" INTEGER NOT NULL,
  "id_playlist" INTEGER NOT NULL,
  "position" INTEGER NOT NULL,
  CONSTRAINT "id_playlist"
    FOREIGN KEY("id_playlist")
    REFERENCES "playlists"("id")
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT "id_song"
    FOREIGN KEY("id_song")
    REFERENCES "songs"("id")
    ON DELETE CASCADE
    ON UPDATE CASCADE
);
CREATE INDEX "playlist_content.id_playlist_idx" ON "playlist_content"("id_playlist");
CREATE INDEX "playlist_content.id_song_idx" ON "playlist_content"("id_song");
COMMIT;
