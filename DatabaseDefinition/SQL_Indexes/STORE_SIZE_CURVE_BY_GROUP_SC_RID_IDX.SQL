--Begin TT#1111 - JScott - Size Curve build database deadlock
create index "STORE_SIZE_CURVE_BY_GROUP_SC_RID_IDX" on "STORE_SIZE_CURVE_BY_GROUP" (
	"SIZE_CURVE_RID")
	
go
