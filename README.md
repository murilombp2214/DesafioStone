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


## Desempenho / Performace
### Os testes de desempenho foram realizados usando jmeter
![image](https://user-images.githubusercontent.com/38633004/110553955-a0171e80-8118-11eb-8161-956c67b6bdee.png)
#### Os scripts e os relatorios dos desempenhos das API(s) se encontram na pasta 'Stone.JMeter' do projeto
##### Abaixo as imagens do desempenho das API(s) nos testes de stress com 700 Thread simultâneas

#### API DE CLIENTE
![image](https://user-images.githubusercontent.com/38633004/110553844-6f36e980-8118-11eb-9382-27ed8cdf0480.png)
#### API DE COBRANÇA
![image](https://user-images.githubusercontent.com/38633004/110553872-79f17e80-8118-11eb-90e0-023e14acc711.png)

###### Como observado as API(s) suportam simultaneamente 700 acessos em ambiente local sem nenhuma falha com um tempo de resposta medio de 1 segundo.

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

##### Cobertura de testes unitarios com coverlet para API DE COBRANÇAS (Apenas camada de dominio)
![image](https://user-images.githubusercontent.com/38633004/110546808-bff51500-810d-11eb-9f96-318f121f2de9.png)

##### Cobertura de testes com integrados coverlet para API DE COBRANÇAS
![image](https://user-images.githubusercontent.com/38633004/110546845-cdaa9a80-810d-11eb-8996-843701e897b2.png)

#### API DE CLIENTES
![image](https://user-images.githubusercontent.com/38633004/110526757-f2dedf00-80f4-11eb-95f8-295e6f7769a5.png)
##### Cobertura de testes unitarios com coverlet para API DE CLIENTES (Apenas camada de dominio)
![image](https://user-images.githubusercontent.com/38633004/110546065-c6cf5800-810c-11eb-9834-fe902ac671f6.png)
##### Cobertura de testes integrados com coverlet para API DE CLIENTES
![image](https://user-images.githubusercontent.com/38633004/110546226-fb431400-810c-11eb-894c-fa22a9ace742.png)


#### BackgroundService (Não se aplica os testes de integração)
![image](https://user-images.githubusercontent.com/38633004/110500266-c799c700-80d7-11eb-94e9-729e7e2f0461.png)


###### O comando utilizado para verificar a cobertura é: dotnet test /p:CollectCoverage=true


