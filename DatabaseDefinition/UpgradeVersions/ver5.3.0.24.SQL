/*RELEASE 5.3.0.24 03-10-2014 - TT#1061-MD -stodd - change msg for Group Allocation */
exec SP_MID_UPDATE_TEXT 080765,'No valid header(s) selected in {0}.',250,0,4,null,0 /* TT#1061-MD - Stodd - valid headers for assortment/GA */
go