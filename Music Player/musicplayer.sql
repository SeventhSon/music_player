SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `musicplayer` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci ;
USE `musicplayer` ;

-- -----------------------------------------------------
-- Table `musicplayer`.`directories`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `musicplayer`.`directories` (
  `id` INT NOT NULL,
  `path` VARCHAR(45) NOT NULL,
  `last_write_time` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `path_UNIQUE` (`path` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `musicplayer`.`songs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `musicplayer`.`songs` (
  `id` INT NOT NULL,
  `path` VARCHAR(45) NOT NULL,
  `artist` VARCHAR(45) NULL,
  `album` VARCHAR(45) NULL,
  `title` VARCHAR(45) NULL,
  `genre` VARCHAR(45) NULL,
  `track_no` INT NULL,
  `rating` INT NULL,
  `length` INT NULL,
  `id_directory` INT NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `path_UNIQUE` (`path` ASC),
  INDEX `id_directory_idx` (`id_directory` ASC),
  CONSTRAINT `id_directory`
    FOREIGN KEY (`id_directory`)
    REFERENCES `musicplayer`.`directories` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `musicplayer`.`playlists`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `musicplayer`.`playlists` (
  `id` INT NOT NULL,
  `title` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `musicplayer`.`playlist_content`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `musicplayer`.`playlist_content` (
  `id_song` INT NOT NULL,
  `id_playlist` INT NOT NULL,
  `position` INT NOT NULL,
  INDEX `id_playlist_idx` (`id_playlist` ASC),
  INDEX `id_song_idx` (`id_song` ASC),
  CONSTRAINT `id_playlist`
    FOREIGN KEY (`id_playlist`)
    REFERENCES `musicplayer`.`playlists` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `id_song`
    FOREIGN KEY (`id_song`)
    REFERENCES `musicplayer`.`songs` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
