/* Add the remaining keys, constraints and indexes for the table "JOB".                       */
alter table "JOB" add constraint "JOB_UC1" unique (
	"JOB_NAME")  


go
