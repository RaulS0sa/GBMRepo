from setuptools import setup, find_packages

VERSION = '0.0.1'
DESCRIPTION = 'GBM Client Library for Python'
LONG_DESCRIPTION = 'This is a client library for the mexican broker GBM(grupo bursatil mexicano) meant to run alongside with trading algoritms'

# Setting up
setup(
    # the name must match the folder name 'verysimplemodule'
    name="GBMProyect",
    version=VERSION,
    author="Raul Sosa",
    author_email="raul.sosa.cortes@gmail.com",
    description=DESCRIPTION,
    long_description=LONG_DESCRIPTION,
    packages=find_packages(),
    install_requires=["pytz", "requests"],
    keywords=['python', 'GBM', "AlgoTrading"],
    classifiers=[
        "Programming Language :: Python :: 3",
        "License :: OSI Approved :: Mozilla Public License 2.0 (MPL 2.0)",
        "Operating System :: OS Independent",
    ]
)