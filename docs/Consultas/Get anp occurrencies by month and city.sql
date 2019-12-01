SELECT DISTINCT ta.ANO, ta.mes, m.nome_municipio 
from tabelaanp ta 
JOIN municipio m 
ON m.id_municipio = ta.id_municipio 
WHERE m.nome_municipio = 'UBA'
AND ta.ANO = '2015'
order by 1, 2;