
### �����ػ�����

```bash
sudo /etc/init.d/supervisor restart 
```


### ��Ŀ���� 


identityserver4:Oauth2��Ȩ��¼
  - ��Ŀ identityserver4/LinCms.IdentityServer4

���к󣬿ɴ�[https://localhost:5003/.well-known/openid-configuration](https://localhost:5003/.well-known/openid-configuration)

token_endpoint:



### ����
- cap Dashboard [https://localhost:5001/cap](https://localhost:5001/cap)



## ����
- lincms-identityserver4 [http://111.231.197.142:5011/swagger/index.html](http://111.231.197.142:5011/swagger/index.html)
- http://111.231.197.142:5010/.well-known/openid-configuration
- lincms-web [http://111.231.197.142:5012/swagger/index.html](http://111.231.197.142:5012/swagger/index.html)
- cap [http://111.231.197.142:5012/cap](http://111.231.197.142:5012/cap)
- Docker����Portainer  [http://47.106.80.39:9000/#/home](http://111.231.197.142:9000/#/home)
- [http://111.231.197.142:5005/healthchecks-ui#/healthchecks](http:/111.231.197.142:5005/healthchecks-ui#/healthchecks)

### �������˺ŵ�¼
- https://localhost:5001/cms/oauth2/signin?provider=Gitee&redirectUrl=https://vvlog.baimocore.cn/
- https://localhost:5001/cms/oauth2/signin?provider=GitHub&redirectUrl=https://vvlog.baimocore.cn/
- https://localhost:5001/cms/oauth2/signin?provider=QQ&redirectUrl=https://vvlog.baimocore.cn/

### �������˺Ű�
- https://localhost:5001/cms/oauth2/signin-bind?provider=GitHub&redirectUrl=https://vvlog.baimocore.cn&token=
- https://localhost:5001/cms/oauth2/signin-bind?provider=QQ&redirectUrl=http://localhost:8081&token=
github���ٵ�¼��ʹ�õ�myget��Դ��

[https://www.myget.org/feed/aspnet-contrib/package/nuget/AspNet.Security.OAuth.GitHub](https://www.myget.org/feed/aspnet-contrib/package/nuget/AspNet.Security.OAuth.GitHub)


//https://github.com/login/oauth/authorize?client_id=0be6b05fc717bfc4fb67&state=github&redirect_uri=https://localhost:5001/signin-github