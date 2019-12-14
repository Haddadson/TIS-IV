
	
	        SELECT mesreferente AS mes, anoreferente AS ano
			              FROM `municipioreferente` M
					              JOIN `tabelausuario` TA
						   	                ON M.id_municipio_referente = TA.id_municipio_referente
									                -- WHERE -- TA.sgdp = 123456 
											                  --  --AND M.anoreferente = 2013
													                   ORDER BY ano ASC, LENGTH(mesreferente), mesreferente ASC;
																	
															        
															        SELECT TA.SGDP, M1.nome_municipio, M2.nome_municipio AS Referente, MR.anoReferente, MR.mesReferente 
																          FROM municipioreferente MR
																			  JOIN municipio M1
																						ON M1.id_municipio = MR.id_municipio
																								  JOIN municipio M2 
																											ON M2.id_municipio = MR.id_municipio_referente
																													  JOIN tabelausuario TA
																													            ON MR.id_municipio = TA.id_municipio
																																   AND MR.id_municipio_referente = TA.id_municipio_referente 
																																         ORDER BY nome_municipio, anoreferente ASC, LENGTH(mesreferente), mesreferente ASC;
																																	        ;
																																			
