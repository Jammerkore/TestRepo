/*  Begin TT#321-MD - JSmith - Basis Size Method db Error */
--alter table "METHOD_SIZE_BASIS_SUBSTITUTES"
--	add constraint "METHOD_SIZE_BASIS_SUBS_PK" primary key ("METHOD_RID")   
alter table "METHOD_SIZE_BASIS_SUBSTITUTES"
	add constraint "METHOD_SIZE_BASIS_SUBS_PK" primary key ("METHOD_RID","SIZE_TYPE_RID","SIZE_TYPE")
/*  End TT#321-MD - JSmith - Basis Size Method db Error */


GO
