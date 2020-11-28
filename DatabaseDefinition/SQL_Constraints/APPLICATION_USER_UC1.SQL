/* Add the remaining keys, constraints and indexes for the table "APPLICATION_USER".          */
alter table "APPLICATION_USER" add constraint "APPLICATION_USER_UC1" unique (
	"USER_NAME")  


go
