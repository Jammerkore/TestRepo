alter table "COMPUTATION_PROCESS"
	add constraint "APP_TEXT_COMP_PROCESS_FK1" foreign key (
		"STATUS_CODE")
	 references "APPLICATION_TEXT" (
		"TEXT_CODE")  

go
