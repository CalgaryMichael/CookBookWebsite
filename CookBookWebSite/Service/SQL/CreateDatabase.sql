USE [master]

IF EXISTS(select * from sys.databases where name = 'CookBookDB')
    DROP DATABASE CookBookDB

CREATE DATABASE [CookBookDB]