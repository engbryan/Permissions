# Projeto
O projeto é pequeno, moderno e está completamente funcional.
É um Empty ASP.NET Framework, com dois DB de exemplo criados na App_Data, e com o EF Context já configurado para um deles (DB do Hotel).

Está sendo utilizado o RESTier para expôr o EF Context através de API RESTful. E está referenciado em código aberto dentro da solução. Está bem escrito, possui documentações e está repleto de comentários.

# Execução
Abrir .sln e executar o projeto.

Com o Postman, requisitar RESTful às entidades do banco de dados (Hotel, Address, HotelGroup).
Exemplo:

GET https://localhost:<porta>/Hotel

POST https://localhost:<porta>/Address
  
{
  PROPRIEDADES DA ENTIDADE ADDRESS
}

DELETE https://localhost:<porta>/Address(1)

GET https://localhost:<porta>/Hotel/1?$select=Title


#RESTier
Repositório:
https://github.com/OData/RESTier

Documentação do RESTier:
http://odata.github.io/RESTier/

Documentação do RESTier no ODATA.ORG
https://www.odata.org/blog/restier-a-turn-key-framework-to-build-restful-service/
