# DesafioStone

## Desenvolvimento
##### O Desenvolvimento da aplicação foi utilizando a linguegem C# junto ao Framework .Net Core 3.1 tendo como Banco de dados MongoDb, foi utilizado também o Swagger para a documentação da API e Docker para utilização de Contêiner.
##### A aplicação final é um BackgroundService que processa os dados de maneira assincrona que se conecta a API de Clientes e apos ter uma remessa de clientes gera as cobranças dos mesmo na API de Cobranças. A aplicação possui uma fila local em memoria apenas para ter o controle de resiliencia aplicado a cobranças não faturadas devido a qualquer tipo de indisponibilidade.

#### Fluxo Sincrono do cenario ideal do BackgroundService
![image](https://user-images.githubusercontent.com/38633004/110498225-e7c88680-80d5-11eb-895a-6b9604a01fe3.png)


#### Variaveis de ambiente
1. Todas as configurações são realizadas via variaveis de ambiente, em ambiente de densenvolvimento
   por padrão o sistema pega as configurações no arquivo '.env' presente no diretorio de cada aplicação.
#### Local
1.
*![image](https://user-images.githubusercontent.com/38633004/110489838-7802cd80-80ce-11eb-805d-623c43a691c6.png)

## Resiliência 
##### A Resiliência aplicada foi utilizando o nuget 'Polly'

 ![image](https://user-images.githubusercontent.com/38633004/110490603-43dbdc80-80cf-11eb-86c7-56828a1d55af.png)

##### Retry
###### O retry é realizado quando uma requisição do 'HttpClient' não consegue ser realizada, então o mesmo ativa a politica de 'Retry'  e faz a retentativa ate que o mesmo consiga uma conexão ou chegue a quantidade maxima de retentativa configurada. O padrão hoje é '5'.

##### Timeout
###### O timeout é realizado quando uma requisição do 'HttpClient' tentar realizar uma conexão e atinge o tempo maxima de espera. O padrão hoje é 3 segundos.

## Monitoramento 
##### O monitoramente da aplicação foi construido com healthcheck 

##### Acesso
###### Para acessar a monitoria basta apenas ir na rota "/HC" que você tera acesso ao Json de status do banco de dados da aplicação
![image](https://user-images.githubusercontent.com/38633004/110492726-3889b080-80d1-11eb-8a28-8d4b2b257bcd.png)

## Logs das aplicações
###### A aplicação registra qualquer tipo de mal funcionamento (erro) ou de algo que necessite de algum tipo de atenção no Console da aplicação em niveis de INFO, WARNING e FAIL.

## Testabilidade
### Testes Unitarios ao lado dos testes de integração realizados com XUnit e Microsoft.AspNetCore.TestHost
#### API DE COBRANÇAS
![image](https://user-images.githubusercontent.com/38633004/110526662-d17df300-80f4-11eb-9db8-94e6282eef0b.png)
#### API DE CLIENTES
![image](https://user-images.githubusercontent.com/38633004/110526757-f2dedf00-80f4-11eb-95f8-295e6f7769a5.png)
#### BackgroundService (Não se aplica os testes de integração)
![image](https://user-images.githubusercontent.com/38633004/110500266-c799c700-80d7-11eb-94e9-729e7e2f0461.png)

#### Testes Unitarios e TDD
###### Os testes unitarios contemplam apenas a camada de dominio da aplicação, sendo a mesma desenvolvida utilizando TDD

