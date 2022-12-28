# Benchmark Rest API
C# console to benchmark a Rest API for Get, Post, Put and Delete verbs.

# Why this tool
Sometimes a benchmark of your API is needed, to test the API wherever is located, in production or localhost, to check whether a refactoring is needed or to investigate some architectural aspects.
A log file and a statistic is generated.

## How to use
It can be run in command line by invoking the command `BenchmarkRest` and passing the following arguments:

`BenchmarkRest myurl numTimes method [startingIteration]`
- *myurl*: Rest API resource
- *numTimes*: number of times to use *myurl* resource.
- *method*: indicates which verb we want to benchmark (Get, Post, Put, Delete). It accepts one the following values (uppercase or lowercase it doesn't matter):
  - Get, g
  - Post, po 
  - Put, pu
  - Delete, del, d
- *json*: parameters used for Api in json format. In particular:
  - Post, Put: json will be passed in Body
  - Get, Delete: json will ne passed in url
- *startingIteration*: optional integer, indicates the starting Id for iteration. It is used only for delete and put verbs in order to seek the Id for update or delete. If this parameter is missing the put and delete will start from initial Id.

## Log file
During the execution of the command a console log will be displayed and a log file is generated at the end of the *numTimes* iterations. 
In order to generate the log file the following packages have been used in the project:
- [ILogger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-6.0) 
- [NLog](https://github.com/NLog/NLog)

## Injections
- IHttpClientFactory 
- ILogger

## Statistics calculation
For each iteration of *numTimes* a stopWatch instance is instantiated and if some data returns with status code 200 the elapsed time is calculated.
The elpased time for each iteration is in memory and at the end of all iterations the following statistics will be calculated:
- max
- min 
- avg

# Use of invoking command:
JsonData contains parameters for the Api. For example by passing the following Json:
`{\"portfolioId\":\"10\"}`
the Api will use portfolioId as parameter with value equals to 10.
`numTimes` values determines then ending value of portfolioId. If `numTimes` equals to 5 the parameter portfolioId will have values between 10 and 15.

In this way, the following commands will use portfolioId as parameter with values between 10 and 30
- `BenchmarkRest https://abc 20 g {\"portfolioId\":\"10\"}`      : 20 times a GET from https://abc 
- `BenchmarkRest https://abc 20 po {\"portfolioId\":\"10\"}`     : 20 times a POST from https://abc

The following commands will use portfolioId as parameter with values between 10 and 30
- `BenchmarkRest https://abc 20 pu 100 {\"portfolioId\":\"10\"}` : 20 times a PUT from https://abc starting from Id = 100 with portfolioId values between 10 and 30
- `BenchmarkRest https://abc 20 d 100 {\"portfolioId\":\"10\"}`  : 20 times a DELETE from https://abc starting from Id = 100 with portfolioId values between 10 and 30

# Results
```
Url: https://abc
OK - iteration 1: 46455 ms at 16.27.57.418
OK - iteration 2: 891 ms at 16.27.58.324
OK - iteration 3: 715 ms at 16.27.59.039
OK - iteration 4: 662 ms at 16.27.59.702
OK - iteration 5: 681 ms at 16.28.00.384
OK - iteration 6: 678 ms at 16.28.01.062
OK - iteration 7: 674 ms at 16.28.01.736
OK - iteration 8: 674 ms at 16.28.02.411
OK - iteration 9: 692 ms at 16.28.03.103
OK - iteration 10: 711 ms at 16.28.03.815

----------------------------------------------
Benchmark for https://abc
Results based on 10 iterations: 
        Avg elased time: 5283.3 ms
        Min elased time: 662 ms
        Max elased time: 46455 ms
----------------------------------------------
```
## Release notes
nlog.config is the configuration file used for log. Edit this file to change name files and path.

## Next steps
- Doing the same by using Python. 
- Generalize POST and DELETE by using an entity passed from command line.

