SELECT PDTYPEDOC FROM itprod.pdpafile WHERE PDTAXID =  '1319900087050'   -- FROM DATA Register 
SELECT * FROM itprod.pdpafile WHERE PDTYPEDOC <> ''

SELECT * FROM  affcdtprod.ocusma where okcono = 200 and okstat = '20' and okcfc0 = 'N' 


SELECT pdtaxid,pdrfcode,pdtaxname,pdtaxaddr,pdphone 
,pdsource ,pdbddt ,pdsexid ,pdnation ,pdrgdt ,pdrgtm
,pdaccdt FROM itprod.pdpafile 
WHERE PDRGDT IS NOT NULL 
AND PDTAXID = '1319900087050' 

ORDER BY PDRGDT desc
LIMIT 50 

 


SELECT PDAFILE FROM itprod.pdpafile WHERE PDTAXID =  '1319900087050'




     select OKVRNO AS TAXID ,okcuno,okcunm
     ,trim(okcua1) || trim(okcua2) || trim(okcua3) || trim(okcua4) as address
     ,okphno,'M3-NFT' as pdsource,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN
     ,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0 
      from affcdtprod.ocusma where okcono = 200 and okstat = '20' and okcfc0 = 'N'  
      and okvrno <> ''  and okvrno not in(select pdtaxid from itprod.pdpafile) 
      
      
      
      select OKVRNO AS TAXID ,okcuno,okcunm   
      ,trim(okcua1) || trim(okcua2) || trim(okcua3) || trim(okcua4) as address  ,okphno,'M3-NFT' as pdsource
      ,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN  
      ,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0   
      from affcdtprod.ocusma where okcono = 200 and okstat = '20' and okcfc0 = 'N'    and okvrno <> ''  
      and okvrno not in(select pdtaxid from itprod.pdpafile)   and okvrno = '1801300118165' 
      
      
      
      
        select idvrno,idsuno
        ,case when sasunm is not null then trim(sasunm) || trim(saadr1) else idsunm end as saname
        ,trim(saadr2) || trim(saadr3) || trim(saadr4) as address,idphno,'M3-NFT' as pdsource 
        ,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN  
        ,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0   
        from affcdtprod.cidmas 
        left join 
        (select sacono,sasuno,sasunm,saadr1,saadr2,saadr3,saadr4,saadte,saadid from affcdtprod.cidadr where saadte = '1' and saadid = 'ADDR') 
        cidadr on sacono = idcono and sasuno = idsuno 
        where idcono = 200 and idstat = '20' and idcfi5 = 'N' 
        and  idvrno <> '' 
        and idvrno not in(select pdtaxid from itprod.pdpafile) 
        and idvrno = '8100400006221'
      
      
        
        select idvrno,idsuno 
        ,case when sasunm is not null then trim(sasunm) || trim(saadr1) else idsunm end as saname 
        ,trim(saadr2) || trim(saadr3) || trim(saadr4) as address
        ,idphno,'M3-NFT' as pdsource 
        ,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN 
        ,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0 
        from affcdtprod.cidmas 
        left join (select sacono,sasuno,sasunm,saadr1,saadr2,saadr3,saadr4,saadte,saadid 
        			from affcdtprod.cidadr where saadte = '1' and saadid = 'ADDR') 
        			cidadr on sacono = idcono and sasuno = idsuno where idcono = 200 and idstat = '20' 
        			and idcfi5 = 'N' and idvrno <> '' 
        and idvrno not in(select pdtaxid from itprod.pdpafile) and idvrno = '8100400006221' 
      
      
      
      
      
       							select OKVRNO AS TAXID,okcuno,okcunm,trim(okcua1) || trim(okcua2) || trim(okcua3) || trim(okcua4) as address,okphno,'M3-PFT' as pdsource
                               ,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN 
        						,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0 
                               from mvxcdtprod.ocusma where okcono = 100 and okstat = '20' and okcfc0 = 'N' 
                               and okvrno <> '' 
                               and okvrno not in(select pdtaxid from itprod.pdpafile) 
      				           and okvrno = '3100400745333' 
      
      				           
      				           
      				           select idvrno,idsuno,case when sasunm is not null then trim(sasunm) || trim(saadr1) else idsunm end as saname,trim(saadr2) || trim(saadr3) || trim(saadr4) as address,idphno,'M3-PFT' as pdsource
                             ,VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN 
        						,REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN ,0 
                            from mvxcdtprod.cidmas 
                            left join 
                            (select sacono,sasuno,sasunm,saadr1,saadr2,saadr3,saadr4,saadte,saadid from MVXcdtprod.cidadr where saadte = '1' and saadid = 'ADDR') 
                            cidadr on sacono = idcono and sasuno = idsuno 
                            where idcono = 100 and idstat = '20' and idcfi5 = 'N' 
                            and idvrno <> '' 
                            and idvrno not in(select pdtaxid from itprod.pdpafile) 
      				        and idvrno = '3301000435760   '   
      				           
      				           
      				           
SELECT idvrno, idsuno
, CASE WHEN sasunm IS NOT NULL THEN TRIM(sasunm) || TRIM(saadr1) ELSE idsunm END AS saname
, TRIM(saadr2) || TRIM(saadr3) || TRIM(saadr4) AS address, idphno, 'M3-PFT' AS pdsource
, VARCHAR_FORMAT(CURRENT TIMESTAMP, 'YYYYMMDD') AS DateN, REPLACE(SUBSTR(CHAR(CURRENT TIME), 1, 8), ':', '') AS TimeN, 0 
FROM mvxcdtprod.cidmas 
LEFT JOIN (  SELECT sacono, sasuno, sasunm, saadr1, saadr2, saadr3, saadr4, saadte, saadid     
			  FROM MVXcdtprod.cidadr     
			  WHERE saadte = '1' AND saadid = 'ADDR' ) 
		 cidadr ON sacono = idcono AND sasuno = idsuno 
		 WHERE idcono = 100 AND idstat = '20' AND idcfi5 = 'N'
		 AND idvrno <> '' 
		 AND idvrno NOT IN (SELECT pdtaxid FROM itprod.pdpafile) 
		 AND idvrno = '3301000435760'
