--
-- PostgreSQL database dump
--

-- Dumped from database version 13.1
-- Dumped by pg_dump version 13.1

-- Started on 2021-05-17 17:08:53

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 200 (class 1259 OID 49153)
-- Name: tour; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tour (
    tourname character varying(100) NOT NULL,
    description character varying(255) NOT NULL,
    tourfrom character varying(100),
    tourto character varying(100),
    distance numeric,
    transportmode character varying(100),
    duration numeric,
    fuelused numeric
);


ALTER TABLE public.tour OWNER TO postgres;

--
-- TOC entry 201 (class 1259 OID 57344)
-- Name: tourlog; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tourlog (
    tourname character varying(100),
    logdate timestamp without time zone,
    report character varying(255),
    rating integer,
    weather character varying(100),
    effort integer,
    duration numeric,
    averagespeed numeric,
    fuelused numeric,
    distance numeric
);


ALTER TABLE public.tourlog OWNER TO postgres;

-- Completed on 2021-05-17 17:08:53

--
-- PostgreSQL database dump complete
--

