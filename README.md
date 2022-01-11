# Benchmark Rest API
C# console to benchmark a Rest API for Get, Post, Put, Delete.

# Why this tool
Sometimes I need to have a benchmark of my APIs, to test after release and to check whether a refactoring is needed.

## How to use
It can be run on then command line passing the following arguments:

`BenchmarkRestGet myurl numTimes method [startingIteration]`
- *myurl*: Rest resource
- *numTimes*: times to access to the resource.
- *method*: indicates which method to benchmark (Get, Post, Put, Delete). It accepts following values (not case sensitive):
  - Get, g
  - Post, po 
  - Put, pu
  - Delete, del, d
- *startingIteration: optional integer, indicates the starting Id for iteration. It is used only for delete and put. If missing the put and delete will start from initial Id.

A console log and a log file is generated. c# [ILogger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-6.0) and [NLog](https://github.com/NLog/NLog) is used.


## How it works
For each time a stopWatch instance is created and if some data returns with status code 200 it calculates the elapsed time.
At the end of all iterations the max, min and avg elpased time is are calculated.

## Uses
- `BenchmarkRest https://abc 10 g`      : 10 times a GET from https://abc 
- `BenchmarkRest https://abc 10 po`     : 10 times a POST from https://abc 
- `BenchmarkRest https://abc 10 pu 100` : 10 times a PUT from https://abc starting from Id = 100
- `BenchmarkRest https://abc 10 d 100`  : 10 times a DELETE from https://abc starting from Id = 100

## Some results
For example if you run from console this command:

`BenchmarkRestGet https://xxxxxxxx 10 g`

it will you will have a resul like this:
```
Url: https://xxxxxxxx
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
Benchmark for https://xxxxxxxx
Results based on 10 iterations: 
        Avg elased time: 5283.3 ms
        Min elased time: 662 ms
        Max elased time: 46455 ms
----------------------------------------------
```

## Next steps
- Doing the same by using Python. 
- Generalize POST and DELETE by using an entity passed from command line.

