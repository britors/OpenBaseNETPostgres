create table clitab (cliid serial not null, clinm varchar(255));
alter table clitab add constraint pk_clitab primary key (cliid);