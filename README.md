CENTRO UNIVERSITÁRIO SENAC

TECNOLOGIA EM ANÁLISE E DENSENVOLVIMENTO DE SISTEMAS
PROJETO INTEGRADOR IV: DESENVOLVIMENTO DE SISTEMAS ORIENTADO A DISPOSITIVOS MÓVEIS E BASEADOS NA WEB


ASSISTENTE DE MEDICAMENTOS E CUIDADOS


ALISSON MEYRELLES

BERNARDO SILVEIRA DA SILVA

DIEGO OLIVEIRA SOUZA

LÍVIA XAVIER SERAPIO DA SILVA

PAULO RIBEIRO DE CARVALHO NETO


BRASIL 2025

------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

EXECUTANDO O PROJETO

# AppSaude (Lembre+) V.0.1.0

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-8.0+-512BD4?logo=.net)](https://dotnet.microsoft.com/apps/maui)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?logo=visual-studio)](https://visualstudio.microsoft.com/)

Breve descrição do seu projeto

## 📋 Pré-requisitos

- [.NET SDK 8.0 ou superior](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) com as cargas de trabalho:
  - **Desenvolvimento para dispositivos móveis com .NET MAUI** 
- Gerenciador de pacotes NuGet
- Dispositivo/simulador para teste (Android)

## 🛠️ Instalação

1. Clone o repositório:
   ```bash
   git clone https://github.com/AMeyrelles/AppSaude.git

Restaure os pacotes NuGet:

bash

dotnet restore
🚀 Executando o Projeto
Abra a solução AppSaude.sln no Visual Studio 2022

Selecione o ambiente de execução na barra superior:

Plataforma: Android

Dispositivo: Selecione emulador/dispositivo físico

Execute o projeto (F5 ou Ctrl + F5)

Para reconstruir completamente (se necessário):

bash

dotnet clean
dotnet build

🗄️
O projeto utiliza SQLite com os seguintes pacotes:

sqlite-net-pcl

SQLitePCLRaw

Para criar/reconstruir o banco de dados:

Verifique os scripts SQL na pasta Scripts/

Execute o projeto - O banco será criado automaticamente no primeiro uso

📂 Estrutura do Projeto

Projeto/

├── Models/          # Modelos de dados

│   ├──  Agendamento.cs
  
│   ├──  Alarme.cs
  
│   ├──  NotificacaoAgendamento.cs
  
│   ├──  NotificacaoAlarme

├── ViewModels/      # ViewModels (MVVM)

│   ├──  AgendamentoViewModel
  
│   ├──  AlarmeViewModel
  
│   ├──  MainViewModel
  
│   ├──  NotificacaoAgendamentoViewModel
  
│   ├──  NotificacaoAlarmeViewModel


├── Views/           # Páginas e componentes UI

│   ├──  AgendamentoView
  
│   ├──  AgendamentoAddView
  
│   ├──  AlarmeView
  
│   ├──  AlarmeAddView
  
│   ├──  HomePageView
  
│   ├──  NotificacaoView

├── Services/        # Serviços e lógica de negócios

│   ├── AlarmeService
  
│   ├──  IAlarmeService
  
│   ├──  IMessage
  
│   ├──  IServiceAndroid
  
│   ├──  IServiceTeste
  
│   ├──  IServiceTeste

├── Resources/       # Assets, estilos e traduções

│   ├──  AppIcon
  
│   ├──  Converters
  
│   ├──  Fonts
  
│   ├──  Images
  
│   ├──  Raw
  
│   ├──  Splash
  
│   ├──  Styles

└──AppSaude.csproj

📦 Dependências Principais
Pacote	Versão	Finalidade
CommunityToolkit.Mvvm	8.4.0	MVVM Helpers
Plugin.LocalNotification	11.1.3	Notificações locais
Plugin.Maui.Audio	3.0.1	Reprodução de áudio
sqlite-net-pcl	1.9.172	ORM SQLite
System.Drawing.Common	8.0.0	Manipulação de imagens
🔧 Configurações Especiais
Notificações Locais
Para configurar as permissões de notificação, verifique:

AppDelegate.cs (iOS)

MainActivity.cs (Android)

MauiProgram.cs (Configuração geral)

Áudio
As configurações de reprodução de áudio requerem:

Permissão no Android (configurar em Platforms/Android/AndroidManifest.xml)

<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE"/>
<uses-permission android:name="android.permission.INTERNET"/>
<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE"/>
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE"/>
<uses-permission android:name="android.permission.POST_NOTIFICATIONS"/>
<uses-permission android:name="android.permission.READ_MEDIA_AUDIO"/>
<uses-permission android:name="android.permission.READ_MEDIA_IMAGES"/>
<uses-permission android:name="android.permission.READ_MEDIA_VIDEO"/>
<uses-permission android:name="com.android.alarm.permission.SET_ALARM"/>
<uses-permission android:name="android.permission.RECEIVE_BOOT_COMPLETED"/>
<uses-permission android:name="android.permission.FOREGROUND_SERVICE"/>
<uses-permission android:name="android.permission.FOREGROUND_SERVICE_DATA_SYNC"/>

//FIM


