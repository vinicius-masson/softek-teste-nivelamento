select    assunto,
          ano,
          count(*) qtd
from      ATENDIMENTOS
group by  assunto,
          ano
having    count(*) > 3
order by  qtd desc