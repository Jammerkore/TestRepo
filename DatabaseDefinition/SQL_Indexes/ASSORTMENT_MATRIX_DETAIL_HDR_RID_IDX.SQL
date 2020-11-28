/* BEGIN TT#90 MD - JSmith - replace primary key with nonunique index */
create index "ASSORTMENT_MATRIX_DETAIL_HDR_RID_IDX" on "ASSORTMENT_MATRIX_DETAIL" (
	"HDR_RID")  

go
