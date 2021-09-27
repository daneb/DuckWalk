# Duckwalk 
A basic web browser written in C# and WPF. Click on image below to see it in action.

[![Video](https://img.youtube.com/vi/ht_DNGmBwyo/hqdefault.jpg)](https://youtu.be/ht_DNGmBwyo)

## Requirements
1. Dotnet Core 3.1
2. Nuget install
3. API key from AbuseIPDB

## Features
1. Multi-Tab support.
2. Site Rating using AbuseIPDB.
3. Site Rating images ![A](Images/icons8-cute-48.png).
4. Setting default search engine (settings files).
5. Back, forward and refresh button
6. Dual support address bar - sites and search

## Caveats
1. Settings does not change Default Search Engine. This is set to DuckDuckGo.
2. There are no general settings.
3. You can create new settings, save and load them.

## Get an API Key for Site Rating API
You can register for a free key from [AbuseIPDB](https://www.abuseipdb.com/).

### How to set environment variable
Powershell export.

```
PS> $env:ABUSEIPDB_API_KEY="f644c5a2ade5a89400165121695a9f239e678d22a4bd12337091263e1d1487cbf20fad9169207bc0"
```
