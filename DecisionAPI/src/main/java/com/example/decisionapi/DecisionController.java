package com.example.decisionapi;

import org.springframework.web.bind.annotation.*;
import weka.classifiers.Evaluation;
import weka.classifiers.bayes.NaiveBayes;
import weka.classifiers.evaluation.Prediction;
import weka.core.Attribute;
import weka.core.DenseInstance;
import weka.core.Instance;
import weka.core.Instances;
import weka.core.converters.ConverterUtils;

import javax.websocket.server.PathParam;
import java.io.Serializable;
import java.util.*;

@RestController
@RequestMapping("/api")
public class DecisionController {

    @RequestMapping("/hello")
    public String hello() {
        return "Hello World";
    }


    //%S1 - more sleep
//%S2 - discuss with someone
//%S3 - take medication
//%A1 - sleep + medication
//%A2 - sleep + discuss with someone
//%A3 - sleep + medication + discuss with someone
//%A4 - discuss with someone + medication

    @PostMapping("/decision")
    public String newDecision(@RequestBody RecommendationDto recommendationDto) throws Exception {

        Recomandation recomandation = new Recomandation(recommendationDto.getQuestionnaireScore(),recommendationDto.getSleepHours(),recommendationDto.isTakeMedication());
        Map<String, String> mappedValues = new HashMap<String, String>();
        mappedValues.put("S1", "We strongly recommend you to get more sleep.");
        mappedValues.put("S2", "You should take in consideration talking to someone about your state");
        mappedValues.put("S3", "We recommend to set an alarm, so you don’t forget to take your medication");
        mappedValues.put("A1", "We strongly recommend you to get more sleep and set an alarm so you don’t\n" +
                "forget about medication");
        mappedValues.put("A2", "We strongly recommend you to get more sleep and you should take in consid-\n" +
                "eration talking to someone about your state.");
        mappedValues.put("A3", " We strongly recommend you to get more sleep, you should take in consideration\n" +
                "talking to someone about your state and don’t forget to set an alarm for medication");
        mappedValues.put("A4", "You should take in consideration talking to someone about your state and don’t\n" +
                "forget to set an alarm for medication");
        mappedValues.put("none", "None");
        ConverterUtils.DataSource source = new ConverterUtils.DataSource("ADL.arff");
        Instances train = source.getDataSet();
        train.setClassIndex(train.numAttributes() - 1);


        NaiveBayes tree = new NaiveBayes();
        tree.buildClassifier(train);
        //Classify
        Attribute depression_severity = new Attribute("depression-severity");
        Attribute sleep = new Attribute("sleep");
        Attribute medication = new Attribute("medication");

        List<String> classVal = new ArrayList<>();
        classVal.add("S1");
        classVal.add("S2");
        classVal.add("S3");
        classVal.add("A1");
        classVal.add("A2");
        classVal.add("A3");
        classVal.add("A4");
        classVal.add("none");

        ArrayList<Attribute> attributes = new ArrayList<>();
        attributes.add(depression_severity);
        attributes.add(sleep);
        attributes.add(medication);
        attributes.add(new Attribute("class", classVal));
        Instances dataRaw = new Instances("TestInstances", attributes, 0);
        dataRaw.setClassIndex(dataRaw.numAttributes() - 1);
        System.out.println("Number of attributes: " + dataRaw.numAttributes());
        double[] instanceValue1 = new double[]{this.convertStatusToIndexDep(recomandation.getDeppresion()),
                this.convertStatusToIndexSleep(recomandation.getSleep()),
                this.convertStatusToIndexMed(recomandation.getMedication())};

        dataRaw.add(new DenseInstance(1.0, instanceValue1));
        System.out.println(dataRaw);
        double result = tree.classifyInstance(dataRaw.instance(0));
        return mappedValues.get(dataRaw.classAttribute().value((int) result));

    }

    public int convertStatusToIndexDep(String status) {
        if (status.equals("mild")) return 1;
        if (status.equals("moderate")) return 2;
        if (status.equals("moderately-severe")) return 3;
        if (status.equals("severe")) return 4;
        return 0;
    }

    public int convertStatusToIndexSleep(String status) {
        if (status.equals("3-5")) return 1;
        if (status.equals("6-8")) return 2;
        if (status.equals("9+")) return 3;
        return 0;

    }

    public int convertStatusToIndexMed(String status) {
        if (status.equals("yes")) return 1;
        return 0;


    }


}

class RecommendationDto implements Serializable {
    public double QuestionnaireScore;
    public int SleepHours;
    public boolean TakeMedication;

    public RecommendationDto(double questionnaireScore, int sleepHours, boolean takeMedication) {
        QuestionnaireScore = questionnaireScore;
        SleepHours = sleepHours;
        TakeMedication = takeMedication;
    }

    public double getQuestionnaireScore() {
        return QuestionnaireScore;
    }

    public void setQuestionnaireScore(double questionnaireScore) {
        QuestionnaireScore = questionnaireScore;
    }

    public int getSleepHours() {
        return SleepHours;
    }

    public void setSleepHours(int sleepHours) {
        SleepHours = sleepHours;
    }

    public boolean isTakeMedication() {
        return TakeMedication;
    }

    public void setTakeMedication(boolean takeMedication) {
        TakeMedication = takeMedication;
    }
}


class Recomandation implements Serializable {
    private String deppresion;
    private String sleep;
    private String medication;

    public String getMedication() {
        return medication;
    }

    public Recomandation(Double deppresion, int sleep, Boolean medication) {
//Decision tree mapping
        if (deppresion <= 4) {
            this.deppresion = "none";
        }
        if (deppresion >= 5 && deppresion <= 9) {
            this.deppresion = "mild";
        }
        if (deppresion >= 10 && deppresion <= 14) {
            this.deppresion = "moderate";
        }
        if (deppresion >= 15 && deppresion <= 19) {
            this.deppresion = "moderately-severe";
        }
        if (deppresion >= 20 && deppresion <= 27) {
            this.deppresion = "severe";
        }

        if(sleep <=2){
            this.sleep = "0-2";
        }
        if(sleep >= 3 && sleep <= 5){
            this.sleep = "3-5";
        }
        if(sleep >= 6 && sleep <=8){
            this.sleep = "6-8";
        }
        if(sleep >=9){
            this.sleep = "9+";
        }

        if(medication){
            this.medication="yes";
        } else {
            this.medication="no";
        }

    }


    public String getDeppresion() {
        return deppresion;
    }

    public void setDeppresion(String deppresion) {
        this.deppresion = deppresion;
    }

    public String getSleep() {
        return sleep;
    }

    public void setSleep(String sleep) {
        this.sleep = sleep;
    }

    public void setMedication(String medication) {
        this.medication = medication;
    }
}
