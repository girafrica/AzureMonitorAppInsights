import java.text.SimpleDateFormat
library 'shared'

pipeline {
    agent any
    stages {
        // stage('Checkout') {
        //     steps {
        //         checkout scm
        //     }
        // }

        // stage('Checkout2') {
        //     steps {
        //         script {
        //             checkoutCode()              
        //             }                
        //         }            
        //     }

        stage('Create version') {
            steps {
                script {
                    currentDateTime = sh script: """
                        date +"v%Y.%V"
                        """.trim(), returnStdout: true
                    version = currentDateTime.trim()  // the .trim() is necessary
                    echo "version: " + version
                }
            }
        }    

        // stage('List tags') {
        //     steps {
        //         script {
        //           int x = 1;
  
        //           //lastTag = sh script: """git tag --list ${version}* --sort=-version:refname | head -1 | grep -oE '[0-9]+\044'""".trim(), returnStdout: true
        //           lastTag = sh script: """git tag --sort=-version:refname | head -1 | grep -oE '[0-9]+\044'""".trim(), returnStdout: true

        //           lt = lastTag.trim()  // the .trim() is necessary
        //           echo "lastTag: " + lt
        //           int lt = lt.toInteger()

        //           newtag = lt + x

        //           echo newtag.toString()
        //         }
        //     }
        // }  

        // stage('Create tag') {
        //     steps {
        //         script {
        //             createTag(version, newtag)
        //         }
        //     }
        // }

        stage('Save tag') {
            steps {
                script {
                    withCredentials([usernamePassword(credentialsId: 'github-app', passwordVariable: 'GIT_PASSWORD', usernameVariable: 'GIT_USERNAME')]) {
                    sh (' ls -l ')
                        dir ('foo'){
                            sh (' ls -l ')                    
                            sh (' git config --global pull.rebase false ')
                            sh (' git fetch https://${GIT_USERNAME}:${GIT_PASSWORD}@github.com/girafrica/release-tags ')
                            sh (' git rebase --abort ')
                            sh (' git pull https://${GIT_USERNAME}:${GIT_PASSWORD}@github.com/girafrica/release-tags --rebase')
                            def readContent = "${version}.sbt"
                            writeFile file: "${version}.sbt", text: readContent+"\r\nversion := 1.0.${env.BUILD_ID}"
                            sh (" git add -A")
                            sh (' git commit -am "Updated version number"')
                            sh (' git push https://${GIT_USERNAME}:${GIT_PASSWORD}@github.com/girafrica/release-tags HEAD:main')
                        }
                    }
                }
            }
        }
    }
}