import numpy
from matplotlib import pyplot


def autocorr(x):
    result = numpy.correlate(x, x, mode='full')
    return result[len(result) // 2:]

def plot_autocorr(a):
    corr = autocorr(a)
    x = range(0, len(corr))
    pyplot.plot(x, corr)
    pyplot.show()

arr = [1, 2, 3, 1, 2, 3]  # <- Use day input

plot_autocorr(arr)

