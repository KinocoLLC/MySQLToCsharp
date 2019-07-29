-- ref: https://github.com/antlr/grammars-v4/blob/master/mysql/examples/ddl_create.sql
-- create table new_t  (like t1);
create table log_table(row varchar(512));
create table ships(name varchar(255), class_id int, id int);
create table ships_guns(guns_id int, ship_id int);
create table guns(id int, power decimal(7,2), callibr decimal(10,3));
create table ship_class(id int, class_name varchar(100), tonange decimal(10,2), max_length decimal(10,2), start_build year, end_build year(4), max_guns_size int);
create table `some table $$`(id int auto_increment key, class varchar(10), data binary) engine=MYISAM;
create table quengine(id int auto_increment key, class varchar(10), data binary) engine='InnoDB';
create table quengine(id int auto_increment key, class varchar(10), data binary) engine="Memory";
create table quengine(id int auto_increment key, class varchar(10), data binary) engine=`CSV`;
create table quengine(id int auto_increment key, class varchar(10), data binary COMMENT 'CSV') engine=MyISAM;
-- create table quengine(id int auto_increment key, class varchar(10), data binary) engine=Aria;
create table `parent_table`(id int primary key, column1 varchar(30), index parent_table_i1(column1(20)), check(char_length(column1)>10)) engine InnoDB;
create table child_table(id int unsigned auto_increment primary key, id_parent int references parent_table(id) match full on update cascade on delete set null) engine=InnoDB;
-- create table `another some table $$` like `some table $$`;
create table `actor` (`last_update` timestamp default CURRENT_TIMESTAMP, `birthday` datetime default CURRENT_TIMESTAMP ON UPDATE LOCALTIMESTAMP);
create table boolean_table(c1 bool, c2 boolean default true);
create table default_table(c1 int default 42, c2 int default -42, c3 varchar(256) DEFAULT _utf8mb3'xxx');
create table ts_table(
  ts1 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  ts2 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE LOCALTIME,
  ts3 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE LOCALTIMESTAMP,
  ts4 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(),
  ts5 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE LOCALTIME(),
  ts6 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE LOCALTIMESTAMP(),
  ts7 TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE NOW(),
  ts8 TIMESTAMP(6) NOT NULL,
  ts9 TIMESTAMP(6) NOT NULL DEFAULT NOW(6) ON UPDATE NOW(6)
);
create table with_check (c1 integer not null,c2 varchar(22),constraint c1 check (c2 in ('a', 'b', 'c')));
CREATE TABLE genvalue1 (id binary(16) NOT NULL, val char(32) GENERATED ALWAYS AS (hex(id)) STORED, PRIMARY KEY (id));
CREATE TABLE genvalue2 (id binary(16) NOT NULL, val char(32) AS (hex(id)) STORED, PRIMARY KEY (id));
CREATE TABLE genvalue3 (id binary(16) NOT NULL, val char(32) GENERATED ALWAYS AS (hex(id)) VIRTUAL, PRIMARY KEY (id));
CREATE TABLE cast_charset (col BINARY(16) GENERATED ALWAYS AS (CAST('xx' as CHAR(16) CHARACTER SET BINARY)) VIRTUAL);
CREATE TABLE cast_charset (col BINARY(16) GENERATED ALWAYS AS (CAST('xx' as CHAR(16) CHARSET BINARY)) VIRTUAL);
CREATE TABLE check_table_kw (id int primary key, upgrade varchar(256), quick varchar(256), fast varchar(256), medium varchar(256), extended varchar(256), changed varchar(256));
CREATE TABLE sercol1 (id SERIAL, val INT);
CREATE TABLE sercol2 (id SERIAL PRIMARY KEY, val INT);
CREATE TABLE sercol3 (id SERIAL NULL, val INT);
CREATE TABLE sercol4 (id SERIAL NOT NULL, val INT);
CREATE TABLE serval1 (id SMALLINT SERIAL DEFAULT VALUE, val INT);
CREATE TABLE serval2 (id SMALLINT SERIAL DEFAULT VALUE PRIMARY KEY, val INT);
CREATE TABLE serval3 (id SMALLINT(3) NULL SERIAL DEFAULT VALUE, val INT);
CREATE TABLE serval4 (id SMALLINT(5) UNSIGNED SERIAL DEFAULT VALUE NOT NULL, val INT);
CREATE TABLE serial (serial INT);
CREATE TABLE float_table (f1 FLOAT, f2 FLOAT(10), f3 FLOAT(7,4));
CREATE TABLE USER (INTERNAL BOOLEAN DEFAULT FALSE);