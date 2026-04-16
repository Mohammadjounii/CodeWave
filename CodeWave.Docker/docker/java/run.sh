#!/bin/bash

# $1 = file name
# $2 = command to run

# Compile Java file
javac Main.java 2> compile_error.txt

if [ -s compile_error.txt ]; then
    echo "ERROR:::"
    cat compile_error.txt
    exit 1
fi

# Run Java program
timeout 5 java Main
