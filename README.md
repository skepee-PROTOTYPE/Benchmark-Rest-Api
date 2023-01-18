# Benchmark Rest API
C# console to benchmark a Rest API with Get, Post, Put and Delete verbs.

# Why this tool
Sometimes a benchmark of your api is needed, and you might need to test the api wherever it is located, maybe your dev environment or production or even on localhost.
This step can be useful in different situations, to check if:
 - your API is still working 
 - a refactoring is needed
 - to investigate some architectural aspects.

This tool through command line helps you to run your api as many times as you want and generates a statistic for you.

## How to use
It can be run in command line by invoking the command `BenchmarkRest` and passing the following arguments:

`BenchmarkRest <url> <numIterations> <httpVerb> <apiParams>`
- *url*: Rest api resource
- *numIterations*: number of times to use *url* resource.
- *httpVerb*: indicates which verb we want to benchmark (Get, Post, Put, Delete). It accepts one the following values (uppercase or lowercase it doesn't matter):
  - Get, g
  - Post, po 
  - Put, pu
  - Delete, del, d
- *apiParams*: optional, used to pass parameters in json format. In particular:
  - Post, Put: params will be passed in Body
  - Get, Delete: params will be passed in url

## Log file
Once the command runs with the right parameters, a console log will be displayed and a log file is generated at the end of the *numIterations* iterations. 
The log file is generated by using the following packages:
- [ILogger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-6.0) 
- [NLog](https://github.com/NLog/NLog)

## Injections
- IHttpClientFactory 
- ILogger

## Statistics calculation
For each iteration of *numIterations* a stopWatch instance is instantiated and if some data returns with status code 200, the elapsed time is calculated.
The elpased time for each iteration is in memory and at the end of all iterations the following statistics will be calculated:
- Max()
- Min() 
- Avg()

# Use of invoking command:
apiParams contains parameters for the api. For example by passing the following Json:
`{\"portfolioId\":\"10\"}`
the api will use `portfolioId` as parameter with value equals to `10`.
`numIterations` determines how many times the api will be invoked. If `numIterations` equals to `5`, `BenchmarkRest` will run with `portfolioId` having values between `10` and `14`.

In this way, the following commands will use `portfolioId` as parameter with values between `10` and `29`
- `BenchmarkRest https://abc 20 g {\"portfolioId\":\"10\"}`  : 20 times a GET from `https://abc` (`https://abc/10` ... `https://abc/29`)
- `BenchmarkRest https://abc 20 po {\"portfolioId\":\"10\"}` : 20 times a POST from `https://abc` passing in body `portfolioId` with values between `10` and `29`
- `BenchmarkRest https://abc 20 pu {\"portfolioId\":\"10\"}` : 20 times a PUT from `https://abc` passing in body `portfolioId` with values between `10` and `29`
- `BenchmarkRest https://abc 20 d {\"portfolioId\":\"10\"}`  : 20 times a DELETE from `https://abc` (`https://abc/10` ... `https://abc/29`)

# Example result
```
Url: https://abc
OK - iteration 1: 46455 ms at 16.27.57.418
OK - iteration 2: 891 ms at 16.27.58.324
...
OK - iteration 10: 711 ms at 16.28.03.815

----------------------------------------------
Benchmark for https://abc
Results based on 10 iterations: 
        Avg elased time: 5283.3 ms
        Min elased time: 662 ms
        Max elased time: 46455 ms
----------------------------------------------
```
## Implicit assumptions for use of this tool
In order to use this tool for your api there are some implicit assumptions:
- `apiParams` values will be passed in Body section for Post and Put
- `apiParams` value will be passed in url for Get and Delete.


## Release notes
nlog.config is the configuration file used for log. Edit this file to change name files and path.

## Next steps
Actually the tool works for a generic API with no authorization key. Next step is to make the tool by passing a Header that at the moment is missing.
Another step is to run the number of iterations asynchronusly.
