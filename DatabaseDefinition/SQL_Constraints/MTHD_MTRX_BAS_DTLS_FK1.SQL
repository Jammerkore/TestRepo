/* Add foreign key constraints to table "METHOD_MATRIX_BASIS_DETAILS".                        */
alter table "METHOD_MATRIX_BASIS_DETAILS"
	add constraint "MTHD_MTRX_BAS_DTLS_FK1" foreign key (
		"METHOD_RID")
	 references "METHOD_MATRIX" (
		"METHOD_RID")  

go
