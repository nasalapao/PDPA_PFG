 SELECT pdtaxid,pdrfcode,pdprefix,pdtaxname,   pdtaxaddr,pdcuraddr
 ,pdworkaddr,   case when pdsexid <> '' then 
 case when pdsexid = '1' then 'Male' else 'Female'  
 end end pdsexid,    pdbddt,pdphone,pdemail,pdlineid
 ,pdbanknm,pdbankno,    case when pdtypedoc <> '' then 
 case when pdtypedoc = '1' then 'à¸¢à¸´à¸?à¸¢à¸­à¸¡' else 'à¹?à¸¡à¹?à¸¢à¸´à¸?à¸¢à¸­à¸¡'    
 end end pdtypedoc,pdaccdt,    case when pdtypedoc2 <> '' 
 then case when pdtypedoc = '1' then 'à¸¢à¸´à¸?à¸¢à¸­à¸¡' else 'à¹?à¸¡à¹?à¸¢à¸´à¸?à¸¢à¸­à¸¡'    
 end end pdtypedoc2,pdaccdt2,    case when pdmthdoc <> '' 
 then case when pdmthdoc = '1' then 'à¸?à¹?à¸²à¸? Web' else 'à¹€à¸­à¸?à¸ªà¸²à¸£ à¹?à¸?à¸?à¸?à¸­à¸£à¹?à¸¡' 
 end end pdmthdoc    
 FROM ITPROD.PDPAFILE 
 WHERE 1 = 1 
 AND  PDACCDT = '20220718'
 ORDER BY PDTAXID,PDRFCODE 

 SELECT  PDTAXID,PDTAXNAME ,PDTAXADDR ,REDATE 
 FROM ITPROD.PDPAFILE A INNER JOIN  ITPROD.PDPAREQUREST B
 ON	a.PDTAXID  = b.RETAXID 
 ORDER  BY REDATE DESC 
 


RESTATUS

SELECT * FROM	PDPAREQUREST
WHERE RETAXID='1319900087050';
 
---à¹?à¸?à¹?à¹?à¸? à¸ à¸²à¸©à¸²à¹?à¸—à¸¢ ----
ALTER TABLE ITPROD.PDPAREQUREST
ALTER COLUMN REMARK SET DATA TYPE VARGRAPHIC(100) CCSID 13488;
 


 INSERT INTO PFGMVX.ITPROD.PDPAREQUREST
(RETAXID, REDATE, REACCESS, REEDIT, REDELETE, RESUSPEND, REOPPOSE, RETRANSFER, REMARK, RESTATUS)
VALUES('1319900087050','2024-11-20' , 0, 0, 0, 0, 0, 0, 'à¸ à¸²à¸©à¸²à¹?à¸—à¸¢', 0);
 
 
 
SELECT *  FROM PFGMVX.ITPROD.PDPAREQUREST
WHERE RETAXID='1319900087050';

UPDATE ITPROD.PDPAREQUREST SET RESTATUS = 4 WHERE RETAXID='1319900087050';
 
 
 INSERT INTO PFGMVX.ITPROD.PDPAREQUREST
(RETAXID)
VALUES('1319900087050');


SELECT  PDTAXID,PDTAXNAME ,PDTAXADDR ,REDATE , 
	CASE B.RESTATUS
        WHEN 0 THEN CAST('à¸£à¸­à¸”à¸³à¹€à¸?à¸´à¸?à¸?à¸²à¸£' AS VARGRAPHIC(20) CCSID 13488)
        WHEN 1 THEN CAST('à¸­à¸¢à¸¹à¹?à¸£à¸°à¸«à¸§à¹?à¸²à¸?à¸”à¸³à¹€à¸?à¸´à¸?à¸?à¸²à¸£' AS VARGRAPHIC(30) CCSID 13488)
        WHEN 2 THEN CAST('à¹€à¸ªà¸£à¹?à¸?à¸ªà¸´à¹?à¸?' AS VARGRAPHIC(20) CCSID 13488)
        WHEN 3 THEN CAST('à¸?à¸?à¸´à¹€à¸ªà¸?' AS VARGRAPHIC(10) CCSID 13488)
        ELSE CAST('à¹?à¸¡à¹?à¸—à¸£à¸²à¸?à¸ªà¸–à¸²à¸?à¸°' AS VARGRAPHIC(20) CCSID 13488)
    END AS RESTATUS 
FROM ITPROD.PDPAFILE A INNER JOIN  ITPROD.PDPAREQUREST B  
ON a.PDTAXID  = b.RETAXID  ORDER  BY REDATE DESC 


 DELETE  ITPROD.PDPAFILE WHERE PDTAXID ='3901000214423' 
 PDAFILE IS NOT NULL AND PDAFILE <> ''


ALTER TABLE ITPROD.PDPAFILE
ALTER COLUMN PDAFILE SET DATA TYPE VARGRAPHIC(500) CCSID 13488;



DELETE  ITPROD.PDPAREQUREST WHERE RETAXID = '3901000214423'

