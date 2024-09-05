# NetCoreOutboxExample

ECommerceOutboxExample, .NET 8 ile Outbox Pattern'in nasıl uygulanacağını gösteren bir örnek mikroservis projesidir. Bu proje, dağıtık sistemlerde veri tutarlılığını sağlamak ve servisler arası güvenilir iletişimi oluşturmak için Outbox Pattern'in nasıl kullanılabileceğini örneklendirmek için oluşturulmuştur.
Daha fazla bilgi için [blog yazısını](https://medium.com/@onurkarasoy/net-core-8-ile-outbox-pattern-uygulamas%C4%B1-f03d0ac0d7ae) ziyaret edebilirsiniz.

## Özellikler

- **Outbox Pattern Uygulaması**: Veri tutarlılığını sağlamak için Outbox Pattern kullanılmıştır.
- **RabbitMQ Entegrasyonu**: Mesajlaşma için RabbitMQ kullanılmıştır.
- **Clean Architecture**: Proje, temiz mimari prensiplerine uygun olarak yapılandırılmıştır.
- **MassTransit Kullanımı**: Mesaj tabanlı iletişim için MassTransit kütüphanesi entegre edilmiştir.
- **Docker Desteği**: RabbitMQ için Docker kullanılarak kolayca kurulum sağlanmıştır.

## Mikroservis Mimarisi ve Outbox Pattern

Mikroservis mimarisi, uygulamaların küçük, bağımsız ve birbirinden bağımsız olarak dağıtılabilen servisler şeklinde yapılandırılmasını sağlar. Bu yapı, veri tutarlılığını sağlamak ve servisler arası güvenilir iletişimi oluşturmak için çeşitli zorluklar barındırır.

**Outbox Pattern**, bu zorlukların üstesinden gelmek için kullanılan etkili yöntemlerden biridir. Outbox Pattern, veritabanı işlemleri ile mesaj gönderme işlemlerini atomik olarak gerçekleştirerek veri tutarlılığını ve mesajlaşmanın güvenilirliğini artırır.

Daha fazla bilgi için [blog yazısını](https://medium.com/@onurkarasoy/net-core-8-ile-outbox-pattern-uygulamas%C4%B1-f03d0ac0d7ae) ziyaret edebilirsiniz.

## Kurulum

### Ön Koşullar

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB kullanılabilir)
- [Git](https://git-scm.com/downloads)

### Projenin Klonlanması

```bash
git clone https://github.com/yourusername/ECommerceOutboxExample.git
cd ECommerceOutboxExample
```

### RabbitMQ Kurulumu

Projenin ana dizininde bulunan `docker-compose.yml` dosyasını kullanarak RabbitMQ'yu Docker ile başlatabilirsiniz.

```bash
docker-compose up -d
```

- **Erişim**: [http://localhost:15672/](http://localhost:15672/) 
- **Kullanıcı Adı**: `guest`
- **Şifre**: `guest`

### Veritabanı Migration

Öncelikle, veritabanı bağlantı dizesini `appsettings.json` dosyasında kontrol edin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceOutboxDb;Trusted_Connection=True;"
},
```

Veritabanı migration işlemlerini gerçekleştirmek için aşağıdaki komutları çalıştırın:

```bash
dotnet ef migrations add AddOutboxTable
dotnet ef database update
```

Bu komutlar, gerekli tabloları veritabanınıza oluşturacaktır.

## Kullanım

Projeyi çalıştırmak için aşağıdaki komutu kullanabilirsiniz:

```bash
dotnet run --project OrderConsumer.Api
```

Bu komut, OrderConsumer.Api projesini başlatır ve RabbitMQ ile veritabanı arasında Outbox Pattern aracılığıyla güvenilir mesajlaşma sağlar.

### Uygulama Akışı

1. **Sipariş Oluşturma**: İşlem başarılı olursa, ilgili event Outbox tablosuna kaydedilir.
2. **Outbox Processor**: Arka planda çalışan OutboxProcessor, işlenmemiş mesajları düzenli aralıklarla kontrol eder.
3. **Mesaj Gönderme**: İşlenmemiş mesajlar RabbitMQ kuyruğuna gönderilir.
4. **Consumer**: Mesaj kuyruğundan gelen mesajları işler ve gerekli iş mantığını gerçekleştirir.

