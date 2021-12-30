# Benchmark Rest API
Simple C# console to benchmark a Rest API for Get, Post, Put, Delete.
Sometimes I need to benchmark my APIs and to check if a refactoring or a re-design post testing is needed.


## How to use
It can be run on then command line passing two arguments:

`BenchmarkRestGet myurl numTimes`
- *myurl*: Rest GET resource
- *numTimes*: times to access to the resource.
- *method*: indicates which method to benchmark (Get, Post, Put, Delete). It accepts following values (not case sensitive):
  - Get, g
  - Post, po 
  - Put, pu
  - Delete, del, d
 


the following 
Alternatively you can pass a third paramter to add a log file:

`BenchmarkRestGet myurl numTimes log`
- *log*: Y/N to add a log file. The file will be saved in the folder where the exe is located. By default it creates the log file.


## How it works
For each time the stopWatch is created and if some data returns with status code 200 it calculates the elapsed time.
At the end of all iterations it calulates the max, min and avg elpased time.

## Some results
For example if you run from console this command:

`BenchmarkRestGet https://xxxxxxxx 10`

you will have a resul like this:

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

## Restrictions
For now it is limited only to GET methods.

## Next steps
- Extending for POST, PUT and DELETE method.
- Doing the same by using Python .

