/*
 Navicat Premium Data Transfer

 Source Server         : 101.43.25.210
 Source Server Type    : MySQL
 Source Server Version : 50740
 Source Host           : 101.43.25.210:3306
 Source Schema         : Personalblog

 Target Server Type    : MySQL
 Target Server Version : 50740
 File Encoding         : 65001

 Date: 01/04/2023 16:04:15
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE IF EXISTS `__EFMigrationsHistory`;
CREATE TABLE `__EFMigrationsHistory`  (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`MigrationId`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for categories
-- ----------------------------
DROP TABLE IF EXISTS `categories`;
CREATE TABLE `categories`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Visible` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for configItems
-- ----------------------------
DROP TABLE IF EXISTS `configItems`;
CREATE TABLE `configItems`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Key` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for featuredCategories
-- ----------------------------
DROP TABLE IF EXISTS `featuredCategories`;
CREATE TABLE `featuredCategories`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CategoryId` int(11) NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `IconCssClass` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_featuredCategories_CategoryId`(`CategoryId`) USING BTREE,
  CONSTRAINT `FK_featuredCategories_categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 4 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for featuredPhotos
-- ----------------------------
DROP TABLE IF EXISTS `featuredPhotos`;
CREATE TABLE `featuredPhotos`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PhotoId` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_featuredPhotos_PhotoId`(`PhotoId`) USING BTREE,
  CONSTRAINT `FK_featuredPhotos_photos_PhotoId` FOREIGN KEY (`PhotoId`) REFERENCES `photos` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for featuredPosts
-- ----------------------------
DROP TABLE IF EXISTS `featuredPosts`;
CREATE TABLE `featuredPosts`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PostId` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_featuredPosts_PostId`(`PostId`) USING BTREE,
  CONSTRAINT `FK_featuredPosts_posts_PostId` FOREIGN KEY (`PostId`) REFERENCES `posts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for photos
-- ----------------------------
DROP TABLE IF EXISTS `photos`;
CREATE TABLE `photos`  (
  `Id` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Location` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `FilePath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Height` bigint(20) NOT NULL,
  `Width` bigint(20) NOT NULL,
  `CreateTime` datetime NOT NULL,
  `YPath` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for posts
-- ----------------------------
DROP TABLE IF EXISTS `posts`;
CREATE TABLE `posts`  (
  `Id` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Summary` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Path` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `CreationTime` datetime NOT NULL,
  `LastUpdateTime` datetime NOT NULL,
  `CategoryId` int(11) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_posts_CategoryId`(`CategoryId`) USING BTREE,
  CONSTRAINT `FK_posts_categories_CategoryId` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for topPosts
-- ----------------------------
DROP TABLE IF EXISTS `topPosts`;
CREATE TABLE `topPosts`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PostId` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `IX_topPosts_PostId`(`PostId`) USING BTREE,
  CONSTRAINT `FK_topPosts_posts_PostId` FOREIGN KEY (`PostId`) REFERENCES `posts` (`Id`) ON DELETE CASCADE ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `Id` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for visitRecords
-- ----------------------------
DROP TABLE IF EXISTS `visitRecords`;
CREATE TABLE `visitRecords`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Ip` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `RequestPath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `RequestQueryString` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `RequestMethod` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `UserAgent` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
  `Time` datetime NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2952 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
